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
        var depositToCreate = model.Adapt<Deposit>();

        await UpdateAccountBalanceForDeposit(model.AccountId, model.Amount);

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


    public async Task UpdateAccountBalanceForDeposit(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account != null)
        {
            account.Balance += amount;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExceedsOperationalLimitForCurrentAccount(int accountId, decimal amount)
    {
        var account = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account.AccountType == EAccountType.Current && account.CurrentAccount != null)
        {
            return amount > account.CurrentAccount.OperationalLimit;
        }

        // Si la cuenta no es de tipo corriente, no se aplica esta validación
        return false;
    }
}
