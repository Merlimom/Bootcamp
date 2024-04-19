using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Core.Exceptions;


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
        // Validar la transferencia
        bool isValid = await ValidateTransfer(model.AccountSourceId, model.AccountDestinationId, model.Amount);

        if (!isValid)
        {
            throw new ValidationException("Movement validation failed.");
        }

        var sourceAccount = await _context.Accounts.FindAsync(model.AccountSourceId);
        sourceAccount.Balance -= model.Amount;

        // Sumar el monto al balance de la cuenta destino
        var destinationAccount = await _context.Accounts.FindAsync(model.AccountDestinationId);
        destinationAccount.Balance += model.Amount;

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

    public async Task<bool> ValidateTransfer(int accountSourceId, int accountDestinationId, decimal amount)
    {
        if (!await IsSameAccountType(accountSourceId, accountDestinationId))
        {
            throw new ValidationException("Las cuentas no son del mismo tipo.");
        }

        if (!await IsSameCurrency(accountSourceId, accountDestinationId))
        {
            throw new ValidationException("Las cuentas no tienen la misma moneda.");
        }

        if (!await IsSufficientBalance(accountSourceId, amount))
        {
            throw new ValidationException("La cuenta de origen no tiene saldo suficiente.");
        }

        if (!await IsSourceAccountActive(accountSourceId))
        {
            throw new ValidationException("La cuenta de origen no está activa.");
        }

        if (await ExceedsOperationalLimit(accountSourceId, amount))
        {
            throw new ValidationException("La transferencia excede el límite operacional de la cuenta de origen.");
        }

        return true;
       
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

        return sourceAccount.Balance >= amount;
    }

    public async Task<bool> IsSourceAccountActive(int sourceAccountId)
    {
        var sourceAccount = await _context.Accounts.FindAsync(sourceAccountId);

        return sourceAccount.AccountStatus == EAccountStatus.Active;
    }

    public async Task<bool> ExceedsOperationalLimit(int sourceAccountId, decimal amount)
    {
        var sourceAccount = await _context.Accounts
       .Include(a => a.CurrentAccount) // Incluye la relación con la cuenta corriente
       .FirstOrDefaultAsync(a => a.Id == sourceAccountId);

        if (sourceAccount.AccountType == EAccountType.Current) // Solo las cuentas corrientes tienen límite operacional
        {
            // Verifica si la cuenta corriente tiene un límite operacional y si la transferencia excede ese límite
            return sourceAccount.CurrentAccount != null && amount > sourceAccount.CurrentAccount.OperationalLimit;
        }

        // Las cuentas de ahorro no tienen límite operacional
        return false;
    }

}
