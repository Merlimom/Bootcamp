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


    public async Task<bool> ExceedsOperationalLimitForCurrentAccount(int accountId, decimal amount, DateTime transactionDate)
    {
        var account = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account?.AccountType == EAccountType.Current && account.CurrentAccount != null)
        {
            decimal totalAmount = await CalculateTotalAmount(accountId, transactionDate);

            return (amount + totalAmount) > account.CurrentAccount.OperationalLimit;
        }

        return false;
    }

    private async Task<decimal> CalculateTotalAmount(int accountId, DateTime transactionDate)
    {
        var account = await _context.Accounts
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null || account.AccountType != EAccountType.Current || account.CurrentAccount == null)
        {
            return 0;
        }

        decimal totalDepositsAmount = await _context.Deposits
            .Where(d => d.AccountId == accountId && d.DepositDateTime.Month == transactionDate.Month)
            .SumAsync(d => d.Amount);

        decimal totalMovementsAmount = await _context.Movements
            .Where(m => (m.AccountSourceId == accountId || m.AccountDestinationId == accountId) &&
                        m.TransferredDateTime.Month == transactionDate.Month)
            .SumAsync(m => m.Amount);

        return totalDepositsAmount + totalMovementsAmount;
    }

    public async Task<bool> DoesAccountExist(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account != null;
    }

    public async Task<bool> DoesBankExist(int bankId)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        return bank != null;
    }

}
