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

        //Consultar el movimiento creado con los detalles de cuenta y moneda incluidos

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

    public async Task<(bool, string)> ExceedsOperationalLimit(int sourceAccountId, int destinationAccountId, decimal amount, DateTime TransferredDateTime)
    {
        var accounts = await _context.Accounts
         .Include(a => a.CurrentAccount)
         .Where(a => a.Id == sourceAccountId || a.Id == destinationAccountId)
         .ToListAsync();

        var sourceAccount = accounts.FirstOrDefault(a => a.Id == sourceAccountId);
        var destinationAccount = accounts.FirstOrDefault(a => a.Id == destinationAccountId);

        if (sourceAccount?.AccountType == EAccountType.Current && sourceAccount.CurrentAccount != null)
        {
            decimal totalSourceMovementsAmount = await CalculateTotalMovementsAmount(sourceAccountId, TransferredDateTime);

            if (amount + totalSourceMovementsAmount > sourceAccount.CurrentAccount.OperationalLimit)
            {
                return (true, "source");
            }
        }

        if (destinationAccount?.AccountType == EAccountType.Current && destinationAccount.CurrentAccount != null)
        {
            decimal totalDestinationMovementsAmount = await CalculateTotalMovementsAmount(destinationAccountId, TransferredDateTime);

            if (amount + totalDestinationMovementsAmount > destinationAccount.CurrentAccount.OperationalLimit)
            {
                return (true, "destination");
            }
        }

        return (false, "not-exceeded");
    }

    private async Task<decimal> CalculateTotalMovementsAmount(int accountId, DateTime transactionDate)
    {
        var account = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null || account.AccountType != EAccountType.Current || account.CurrentAccount == null)
        {
            // Si la cuenta no es de tipo Current o no tiene un CurrentAccount asociado, retornar 0
            return 0;
        }

        decimal totalMovementsAmount = await _context.Movements
            .Where(m => (m.AccountSourceId == accountId || m.AccountDestinationId == accountId) &&
                        m.TransferredDateTime.Month == transactionDate.Month)
            .SumAsync(m => m.Amount);

        decimal totalDepositsAmount = await _context.Deposits
            .Where(d => d.AccountId == accountId && d.DepositDateTime.Month == transactionDate.Month)
            .SumAsync(d => d.Amount);

        return totalMovementsAmount + totalDepositsAmount;
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

        sourceAccount!.Balance -= amount;
        destinationAccount!.Balance += amount;

        await _context.SaveChangesAsync();
    }

}


