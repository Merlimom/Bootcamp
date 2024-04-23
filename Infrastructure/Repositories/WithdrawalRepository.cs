using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly BootcampContext _context;

    public WithdrawalRepository(BootcampContext context)
    {
        _context = context;
    }
    public async Task<WithdrawalDTO> Add(CreateWithdrawalModel model)
    {
        var withdrawalToCreate = model.Adapt<Withdrawal>();

        await UpdateAccountBalanceForWithdrawal(model.AccountId, model.Amount);


        _context.Withdrawals.Add(withdrawalToCreate);

        var result = await _context.SaveChangesAsync();

        var query = await _context.Withdrawals
            .Include(a => a.Account)
                .ThenInclude(a => a.Customer)
                    .ThenInclude(a => a.Bank)
            .SingleOrDefaultAsync(r => r.Id == withdrawalToCreate.Id);

        var withdrawalDTO = query.Adapt<WithdrawalDTO>();
        return withdrawalDTO;
    }

    public async Task UpdateAccountBalanceForWithdrawal(int accountId, decimal amount)
    {

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account != null)
        {
            account.Balance -= amount;
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
        return false;
    }

    public async Task<List<WithdrawalDTO>> GetAll()
    {
        var query = _context.Withdrawals
                       .Include(a => a.Account)
                       .AsQueryable();

        var deposits = await _context.Withdrawals.ToListAsync();

        var withdrawalDTO = deposits.Adapt<List<WithdrawalDTO>>();

        return withdrawalDTO;
    }
}