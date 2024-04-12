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

        var customers = await _context.Customers.ToListAsync();
        var banks = await _context.Banks.ToListAsync();
        var currencies = await _context.Currencies.ToListAsync();
        var savingAccounts = await _context.SavingAccounts.ToListAsync();
        var currentAccounts = await _context.CurrentAccounts.ToListAsync();

        //var accountToCreate = model.Adapt<Account>();

        //// Asigna las propiedades de la cuenta comunes a todas las cuentas
        //accountToCreate.Holder = model.Holder;
        //accountToCreate.Number = model.Number;
        //accountToCreate.CurrencyId = model.CurrencyId;
        //accountToCreate.CustomerId = model.CustomerId;

        //// Agregar lógica para determinar el tipo de cuenta y guardarla en la tabla correspondiente
        //if (model.AccountType == "Current")
        //{
        //    var currentAccount = accountToCreate.Adapt<CurrentAccount>();
        //    currentAccount.OperationalLimit = model.OperationalLimit;
        //    currentAccount.MonthAverage = model.MonthAverage;
        //    currentAccount.Interest = model.Interest;

        //    _context.CurrentAccounts.Add(currentAccount);
        //}
        //else if (model.AccountType == "Saving")
        //{
        //    var savingAccount = accountToCreate.Adapt<SavingAccount>();
        //    savingAccount.SavingType = model.SavingType;
        //    savingAccount.HolderName = model.HolderName;

        //    _context.SavingAccounts.Add(savingAccount);
        //}

        //await _context.SaveChangesAsync();

        //var accountDTO = accountToCreate.Adapt<AccountDTO>();

        //return accountDTO;

        var accountToCreate = model.Adapt<Account>();

        // Agregar lógica para determinar el tipo de cuenta y guardarla en la tabla correspondiente
        if (model.AccountType == "Current")
        {
            var currentAccount = accountToCreate.Adapt<CurrentAccount>();
            _context.CurrentAccounts.Add(currentAccount);
        }
        else if (model.AccountType == "Saving")
        {
            var savingAccount = accountToCreate.Adapt<SavingAccount>();
            _context.SavingAccounts.Add(savingAccount);
        }

        await _context.SaveChangesAsync();

        var accountDTO = accountToCreate.Adapt<AccountDTO>();

        return accountDTO;


        //var accountToCreate = model.Adapt<Account>();

        //_context.Accounts.Add(accountToCreate);

        //await _context.SaveChangesAsync();

        //var accountDTO = accountToCreate.Adapt<AccountDTO>();

        //return accountDTO;

        //var customer = await _context.Customers
        //     .Include(c => c.Bank)
        //     .FirstOrDefaultAsync(c => c.Id == model.CustomerId);

        //if (customer is null) throw new NotFoundException($"Customer with id: {model.CustomerId} not found");

        //var currency = await _context.Currencies.FindAsync(model.CurrencyId);
        //if (currency is null) throw new NotFoundException($"Currency with id: {model.CurrencyId} not found");

        //var accountToCreate = model.Adapt<Account>();

        //_context.Accounts.Add(accountToCreate);

        //await _context.SaveChangesAsync();

        //var createdAccount = await _context.Accounts
        //    .Include(x => x.Customer)
        //    .Include(x => x.Currency)
        //    .FirstOrDefaultAsync(x => x.Id == accountToCreate.Id);

        //var accountDTO = createdAccount.Adapt<AccountDTO>();

        //return accountDTO;
    }

    public async Task<bool> Delete(int id)
    {
        var creditcard = await _context.CreditCards.FindAsync(id);

        if (creditcard is null) throw new Exception("Credit Card not found");

        _context.CreditCards.Remove(creditcard);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<AccountDTO>> GetFiltered(FilterAccountModel filter)
    {
        var customers = await _context.Customers.ToListAsync();
        var banks = await _context.Banks.ToListAsync();
        var currencies = await _context.Currencies.ToListAsync();

        var query = _context.Accounts
            .Include(c => c.Customer)
            .AsQueryable();

        var queryCurrency = _context.Accounts
            .Include(c => c.Currency)
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

        if (filter.Type is not null)
        {
            query = query.Where(x =>
            x.AccountType == filter.Type);
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
        var customers = await _context.Customers.ToListAsync();
        var banks = await _context.Banks.ToListAsync();
        var currencies = await _context.Currencies.ToListAsync();

        var account = await _context.Accounts.FindAsync(model.Id);

        if (account is null) throw new Exception("Account was not found");

        model.Adapt(account);

        _context.Accounts.Update(account);

        await _context.SaveChangesAsync();

        var accountDTO = account.Adapt<AccountDTO>();

        return accountDTO;
    }

    public async Task<bool> VerifyCustomerExists(int id)
    {
        var customerDoesntExist = await _context.Customers.AnyAsync(customer => customer.Id == id);

        return !customerDoesntExist;

    }

    public async Task<bool> VerifyCurrencyExists(int id)
    {
        var currencyDoesntExist = await _context.Currencies.AnyAsync(currency => currency.Id == id);

        return !currencyDoesntExist;

    }
}
