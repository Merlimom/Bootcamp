
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
        //await _movementRepository.UpdateOperationalLimitByMonth(accountSourceId);
        //await _movementRepository.UpdateOperationalLimitByMonth(accountDestinationId);

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


        //var currentDate = DateTime.UtcNow;

        //var sourceAccount = await _context.Accounts
        //    .Include(a => a.Movements)
        //    .Include(a => a.CurrentAccount)
        //    .FirstOrDefaultAsync(a => a.Id == sourceAccountId);

        //var destinationAccount = await _context.Accounts
        //    .Include(a => a.Movements)
        //    .Include(a => a.CurrentAccount)
        //    .FirstOrDefaultAsync(a => a.Id == destinationAccountId);

        //sourceAccount.Balance -= amount;
        //destinationAccount.Balance += amount;

        //// Verifica si la cuenta tiene una cuenta corriente asociada antes de acceder a CurrentAccount
        //if (sourceAccount.CurrentAccount != null && destinationAccount.CurrentAccount != null)
        //{
        //    // Verifica si la fecha de la última transferencia es anterior al mes actual para sourceAccount
        //    var lastSourceTransferDate = sourceAccount.CurrentAccount?.Account.Movements
        //           .Where(m => m.AccountSourceId == sourceAccountId) // Filtrar por la cuenta de origen
        //           .OrderByDescending(m => m.TransferredDateTime)
        //           .FirstOrDefault()?.TransferredDateTime;


        //    // Verifica si la fecha de la última transferencia es anterior al mes actual para destinationAccount
        //    var lastDestinationTransferDate = destinationAccount.CurrentAccount?.Account.Movements
        //           .Where(m => m.AccountDestinationId == destinationAccountId) // Filtrar por la cuenta de destino
        //           .OrderByDescending(m => m.TransferredDateTime)
        //           .FirstOrDefault()?.TransferredDateTime;

        //    // Si la fecha de la última transferencia es diferente al mes actual, restablece el límite operacional
        //    if (lastSourceTransferDate?.Month != currentDate.Month)
        //    {
        //        sourceAccount.CurrentAccount.OperationalLimit = sourceAccount.CurrentAccount.InitialOperationalLimit;
        //    }

        //    if (lastDestinationTransferDate?.Month != currentDate.Month)
        //    {
        //        destinationAccount.CurrentAccount.OperationalLimit = destinationAccount.CurrentAccount.InitialOperationalLimit;
        //    }

        //    // Verifica si hay suficiente saldo y límite operativo disponible para realizar la operación
        //    if (sourceAccount.Balance >= amount && sourceAccount.CurrentAccount.OperationalLimit >= amount)
        //    {
        //        // Restringe la disminución del límite operacional si es menor que la cantidad en la transferencia
        //        sourceAccount.CurrentAccount.OperationalLimit = Math.Max(sourceAccount.CurrentAccount.OperationalLimit - amount, 0);
        //        destinationAccount.CurrentAccount.OperationalLimit = Math.Max(destinationAccount.CurrentAccount.OperationalLimit - amount, 0);

        //        await _context.SaveChangesAsync();
        //    }


        //var currentDate = DateTime.UtcNow;

        //var sourceAccount = await _context.Accounts
        //    .Include(a => a.Movements)
        //    .Include(a => a.CurrentAccount)
        //    .FirstOrDefaultAsync(a => a.Id == sourceAccountId);

        //var destinationAccount = await _context.Accounts
        //    .Include(a => a.Movements)
        //    .Include(a => a.CurrentAccount)
        //    .FirstOrDefaultAsync(a => a.Id == destinationAccountId);

        //sourceAccount.Balance -= amount;
        //destinationAccount.Balance += amount;



        //// Verifica si la cuenta tiene una cuenta corriente asociada antes de acceder a CurrentAccount
        //if (sourceAccount.CurrentAccount != null && destinationAccount.CurrentAccount != null)
        //{
        //    // Verifica si la fecha de la última transferencia es anterior al mes actual para sourceAccount
        //    var lastSourceTransferDate = sourceAccount.CurrentAccount?.Account.Movements
        //        .Where(m => m.AccountSourceId == sourceAccountId) // Filtrar por la cuenta de origen
        //        .OrderByDescending(m => m.TransferredDateTime)
        //        .FirstOrDefault()?.TransferredDateTime;

        //    // Verifica si la fecha de la última transferencia es anterior al mes actual para destinationAccount
        //    var lastDestinationTransferDate = destinationAccount.CurrentAccount?.Account.Movements
        //        .Where(m => m.AccountDestinationId == destinationAccountId) // Filtrar por la cuenta de destino
        //        .OrderByDescending(m => m.TransferredDateTime)
        //        .FirstOrDefault()?.TransferredDateTime;

        //    // Si la fecha de la última transferencia es diferente al mes actual, restablece el límite operacional
        //    if (lastSourceTransferDate?.Month != currentDate.Month)
        //    {
        //        sourceAccount.CurrentAccount.OperationalLimit = sourceAccount.CurrentAccount.InitialOperationalLimit;
        //    }

        //    if (lastDestinationTransferDate?.Month != currentDate.Month)
        //    {
        //        destinationAccount.CurrentAccount.OperationalLimit = destinationAccount.CurrentAccount.InitialOperationalLimit;
        //    }

        //    // Verifica si hay suficiente saldo y límite operativo disponible para realizar la operación
        //    if (sourceAccount.Balance >= amount && sourceAccount.CurrentAccount.OperationalLimit >= amount)
        //    {
        //        // Restringe la disminución del límite operacional si es menor que la cantidad en la transferencia
        //        sourceAccount.CurrentAccount.OperationalLimit = Math.Max(sourceAccount.CurrentAccount.OperationalLimit - amount, 0);
        //        destinationAccount.CurrentAccount.OperationalLimit = Math.Max(destinationAccount.CurrentAccount.OperationalLimit - amount, 0);

        //        await _context.SaveChangesAsync();
    }
    public async Task<bool> UpdateOperationalLimitByMonth()
    {
        try
        {
            var currentDate = DateTime.UtcNow;

            // Obtener todas las cuentas actuales (de tipo "current") con movimientos
            var accounts = await _context.Accounts
                .Include(a => a.CurrentAccount)
                .Include(a => a.Movements)
                .Where(a => a.AccountType == EAccountType.Current && a.Movements.Any())
                .ToListAsync();

            foreach (var account in accounts)
            {
                // Verificar si hay movimientos registrados en el mes actual
                var lastTransferDate = account.Movements
                    .Where(m => m.TransferredDateTime.Value.Month == currentDate.Month)
                    .OrderByDescending(m => m.TransferredDateTime)
                    .FirstOrDefault()?.TransferredDateTime;

                // Si no hay movimientos para el mes actual, restablecer el límite operacional
                if (lastTransferDate == null)
                {
                    account.CurrentAccount.OperationalLimit = account.CurrentAccount.InitialOperationalLimit;
                }
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir
            // Aquí puedes registrar el error, lanzar una excepción o manejarlo de otra manera según tu necesidad
            Console.WriteLine($"Error al actualizar el límite operacional por mes: {ex.Message}");
            return false;
        }
    }

}






//public async Task<bool> ExceedsOperationalLimitForCurrentAccount(int destinationAccountId, decimal amount)
//{
//    var destinationAccount = await _context.Accounts
//        .Include(a => a.CurrentAccount)
//        .FirstOrDefaultAsync(a => a.Id == destinationAccountId);

//    if (destinationAccount.AccountType == EAccountType.Current && destinationAccount.CurrentAccount != null)
//    {
//        return amount > destinationAccount.CurrentAccount.OperationalLimit;
//    }

//    // Si la cuenta no es de tipo corriente, no se aplica esta validación
//    return false;


