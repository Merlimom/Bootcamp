using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using FluentValidation;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DepositRepository : IDepositRepository
{
    private readonly BootcampContext _context;

    public DepositRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<DepositDTO> Add(CreateDepositModel model)
    {
        await ProcessDeposit(model);

        var depositToCreate = model.Adapt<Deposit>();

        _context.Deposits.Add(depositToCreate);

        var result = await _context.SaveChangesAsync();

        var query = await _context.Deposits
            .Include(a => a.Account)
                .ThenInclude(a => a.Customer)
                    .ThenInclude(a => a.Bank)
            .SingleOrDefaultAsync(r => r.Id == depositToCreate.Id);

        var depositDTO = query.Adapt<DepositDTO>();
        return depositDTO;
    }

    public async Task<List<DepositDTO>> GetAll()
    {
        var query = _context.Deposits
                .Include(a => a.Account)
                .ThenInclude(a => a.Customer)
                .ThenInclude(a => a.Bank)
                .AsQueryable();

        var deposits = await _context.Deposits.ToListAsync();

        var depositDTO = deposits.Adapt<List<DepositDTO>>();

        return depositDTO;
    }


    public async Task UpdateAccountBalanceForDeposit(int destinationAccountId, decimal amount)
    {
        var destinationAccount = await _context.Accounts.FindAsync(destinationAccountId);
        destinationAccount.Balance += amount;
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

    public async Task ProcessDeposit(CreateDepositModel model)
    {
        // Incrementar el saldo de la cuenta de destino
        await UpdateAccountBalanceForDeposit(model.AccountId, model.Amount);

        // Verificar si se excede el límite operacional para cuentas corrientes
        if (await ExceedsOperationalLimitForCurrentAccount(model.AccountId, model.Amount))
        {
            throw new ValidationException("Deposit exceeds the operational limit for the destination account.");
        }
    }
}
