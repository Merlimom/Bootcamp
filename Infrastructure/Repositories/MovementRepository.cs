
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using FluentValidation;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class MovementRepository : IMovementRepository
{
    private readonly BootcampContext _context;

    public MovementRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<MovementDTO> Add(CreateMovementModel model)
    {
        await UpdateAccountBalancesAndLimits(model.AccountSourceId, model.AccountDestinationId, model.Amount);

        // Convertir el modelo a una entidad Movement
        var movementToCreate = model.Adapt<Movement>();

        // Agregar el movimiento a la base de datos
        _context.Movements.Add(movementToCreate);

        var result = await _context.SaveChangesAsync();

        // Consultar el movimiento creado con los detalles de cuenta y moneda incluidos
        var query = await _context.Movements
            .Include(a => a.Account)
                .ThenInclude(a => a.Customer)
                    .ThenInclude(a => a.Bank)
            .Include(a => a.Account)
                .ThenInclude(a => a.Currency)
            .SingleOrDefaultAsync(r => r.Id == movementToCreate.Id);

        // Convertir el movimiento consultado a DTO y devolverlo
        var movementDTO = query.Adapt<MovementDTO>();
        return movementDTO;
    }

    public async Task<List<MovementDTO>> GetAll()
    {
        var query = _context.Movements
         .Include(a => a.Account)
         .ThenInclude(a => a.Customer)
         .ThenInclude(a => a.Bank)
         .Include(a => a.Account)
         .ThenInclude(a => a.Currency)
         .AsQueryable();

        var result = await query.ToListAsync();


        var movements = await _context.Movements.ToListAsync();

        var movementDTO = movements.Adapt<List<MovementDTO>>();

        return movementDTO;
    }

    public async Task<bool> IsSameAccountType(int sourceAccountId, int destinationAccountId)
    {
        var sourceAccountType = await _context.Accounts
            .Where(a => a.Id == sourceAccountId)
            .Select(a => a.AccountType)
            .FirstOrDefaultAsync();

        var destinationAccountType = await _context.Accounts
            .Where(a => a.Id == destinationAccountId)
            .Select(a => a.AccountType)
            .FirstOrDefaultAsync();

        return sourceAccountType == destinationAccountType;
    }

    public async Task<bool> IsSameCurrency(int sourceAccountId, int destinationAccountId)
    {
        var sourceCurrency = await _context.Accounts
            .Where(a => a.Id == sourceAccountId)
            .Select(a => a.Currency)
            .FirstOrDefaultAsync();

        var destinationCurrency = await _context.Accounts
            .Where(a => a.Id == destinationAccountId)
            .Select(a => a.Currency)
            .FirstOrDefaultAsync();

        return sourceCurrency == destinationCurrency;
    }

    public async Task<bool> IsSufficientBalance(int sourceAccountId, decimal amount)
    {
        var sourceAccount = await _context.Accounts.FindAsync(sourceAccountId);

        return sourceAccount?.Balance >= amount;
    }

    public async Task<bool> IsSourceAccountActive(int sourceAccountId)
    {
        var sourceAccount = await _context.Accounts.FindAsync(sourceAccountId);

        return sourceAccount?.AccountStatus == EAccountStatus.Active;
    }

    public async Task<(bool, string)> ExceedsOperationalLimit(int sourceAccountId, int destinationAccountId, decimal amount)
    {
        var accounts = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .Where(a => a.Id == sourceAccountId || a.Id == destinationAccountId)
            .ToListAsync();

        var sourceAccount = accounts.FirstOrDefault(a => a.Id == sourceAccountId);
        var destinationAccount = accounts.FirstOrDefault(a => a.Id == destinationAccountId);

        if (sourceAccount?.AccountType == EAccountType.Current && sourceAccount.CurrentAccount != null)
        {
            if (amount > sourceAccount.CurrentAccount.OperationalLimit)
            {
                return (true, "source");
            }
        }

        if (destinationAccount?.AccountType == EAccountType.Current && destinationAccount.CurrentAccount != null)
        {
            if (amount > destinationAccount.CurrentAccount.OperationalLimit)
            {
                return (true, "destination");
            }
        }

        //return (false, null);
        return (false, "not-exceeded");
    }


    public async Task<bool> IsSameBank(int sourceAccountId, int destinationAccountId)
    {
        var accounts = await _context.Accounts
            .Where(a => a.Id == sourceAccountId || a.Id == destinationAccountId)
            .Include(a => a.Customer)
                .ThenInclude(c => c.Bank)
            .ToListAsync();

        var sourceAccount = accounts.FirstOrDefault(a => a.Id == sourceAccountId);
        var destinationAccount = accounts.FirstOrDefault(a => a.Id == destinationAccountId);

        if (sourceAccount?.Customer?.Bank == null || destinationAccount?.Customer?.Bank == null)
        {
            return false;
        }

        return sourceAccount.Customer.Bank.Id == destinationAccount.Customer.Bank.Id;
    }

    public async Task UpdateAccountBalancesAndLimits(int sourceAccountId, int destinationAccountId, decimal amount)
    {
        var sourceAccount = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == sourceAccountId);
        var destinationAccount = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == destinationAccountId);

        sourceAccount.Balance -= amount;
        destinationAccount.Balance += amount;

        if (sourceAccount?.AccountType == EAccountType.Current && sourceAccount.CurrentAccount != null)
        {
            sourceAccount.CurrentAccount.OperationalLimit -= amount;
        }

        if (destinationAccount?.AccountType == EAccountType.Current && destinationAccount.CurrentAccount != null)
        {
            destinationAccount.CurrentAccount.OperationalLimit -= amount;
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAccountBalanceForWithdrawal(int sourceAccountId, decimal amount)
    {
        var sourceAccount = await _context.Accounts.FindAsync(sourceAccountId);

        if (sourceAccount == null)
        {
            throw new NotFoundException("Source account not found.");
        }

        // Verificar si la cuenta tiene fondos suficientes para la extracción
        if (sourceAccount.Balance < amount)
        {
            throw new ValidationException("Insufficient funds for withdrawal.");
        }

        // Decrementar el saldo de la cuenta
        sourceAccount.Balance -= amount;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExceedsOperationalLimitForCurrentAccount(int destinationAccountId, decimal amount)
    {
        var destinationAccount = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == destinationAccountId);

        if (destinationAccount.AccountType == EAccountType.Current && destinationAccount.CurrentAccount != null)
        {
            return amount > destinationAccount.CurrentAccount.OperationalLimit;
        }

        // Si la cuenta no es de tipo corriente, no se aplica esta validación
        return false;
    }

    public async Task ProcessWithdrawal(CreateMovementModel model)
    {
        // Verificar si se excede el límite operacional para cuentas corrientes
        if (await ExceedsOperationalLimitForCurrentAccount(model.AccountSourceId, model.Amount))
        {
            throw new ValidationException("Withdrawal exceeds the operational limit for the source account.");
        }

        // Decrementar el saldo de la cuenta de origen
        await UpdateAccountBalanceForWithdrawal(model.AccountSourceId, model.Amount);
    }



}
