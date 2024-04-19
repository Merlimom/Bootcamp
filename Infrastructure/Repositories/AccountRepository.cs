using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BootcampContext _context;

    public AccountRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<AccountDTO> Add(CreateAccountModel model)
    {

        var account = model.Adapt<Account>();

        if (account.AccountType == EAccountType.Saving)
        {
            account.SavingAccount = model.CreateSavingAccountModel.Adapt<SavingAccount>();
        }

        if (account.AccountType == EAccountType.Current)
        {
            account.CurrentAccount = model.CreateCurrentAccountModel.Adapt<CurrentAccount>();
        }

        _context.Accounts.Add(account);

        await _context.SaveChangesAsync();

        var createdAccount = await _context.Accounts
            .Include(a => a.Currency)
            .Include(a => a.Customer)
            .ThenInclude(a => a.Bank)
            .Include(a => a.SavingAccount)
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(a => a.Id == account.Id);

        return createdAccount.Adapt<AccountDTO>();
    }

    public async Task<AccountDTO> GetById(int id)
    {
        var account = await _context.Accounts
            .Include(a => a.Currency)
            .Include(a => a.Customer)
            .ThenInclude(a => a.Bank)
            .Include(a => a.SavingAccount)
            .Include(a => a.CurrentAccount)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (account is null) throw new NotFoundException($"The account with id: {id} doest not exist");

        return account.Adapt<AccountDTO>();

    }

    public async Task<bool> Delete(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account is null) throw new NotFoundException("Account with ID " + id + " was not found");

        account.IsDeleted = EIsDeletedStatus.True;

        account.AccountStatus = EAccountStatus.Inactive;

        var result = await _context.SaveChangesAsync();

        return result > 0; ;
    }

    public async Task<List<AccountDTO>> GetFiltered(FilterAccountModel filter)
    {

        var query = _context.Accounts
                  .Include(a => a.Currency)
                  .Include(a => a.Customer)
                  .ThenInclude(a => a.Bank)
                  .Include(a => a.SavingAccount)
                  .Include(a => a.CurrentAccount)
                  .AsQueryable();



        if (filter.CustomerId is not null)
        {
            query = query.Where(x =>
                (x.CustomerId).Equals(filter.CustomerId));
        }

        if (filter.CurrencyId is not null)
        {
            query = query.Where(x =>
                (x.CurrencyId).Equals(filter.CurrencyId));
        }

        if (filter.AccountType is not null)
        {
            query = query.Where(x =>
            x.AccountType == filter.AccountType);
        }

        if (filter.Number is not null)
        {
            query = query.Where(x =>
                 x.Number != null &&
                (x.Number).Equals(filter.Number));
        }

        var result = await query.ToListAsync();

        var accountDTO = result.Adapt<List<AccountDTO>>();
        return accountDTO;
    }

    public async Task<AccountDTO> Update(UpdateAccountModel model)
    {

        var query = _context.Accounts
                  .Include(a => a.Currency)
                  .Include(a => a.Customer)
                  .ThenInclude(a => a.Bank)
                  .Include(a => a.SavingAccount)
                  .Include(a => a.CurrentAccount)
                  .AsQueryable();

        var result = await query.ToListAsync();

        var account = await _context.Accounts.FindAsync(model.Id);

        if (account is null) throw new Exception("Account was not found");

        model.Adapt(account);

        _context.Accounts.Update(account);

        await _context.SaveChangesAsync();

        var accountDTO = account.Adapt<AccountDTO>();

        return accountDTO;
    }

  public async Task<(bool customerExists, bool currencyExists)> VerifyCustomerAndCurrencyExist(int customerId, int currencyId)
    {
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == currencyId);

        return (customerExists, currencyExists);
    }
}
