using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CreditCardRepository : ICreditCardRepository
{
    private readonly BootcampContext _context;

    public CreditCardRepository(BootcampContext context)
    {
        _context = context;
    }
    public async Task<CreditCardDTO> Add(CreateCreditCardModel model)
    {

        var customers = await _context.Customers.ToListAsync();
        var banks = await _context.Banks.ToListAsync();
        var currencies = await _context.Currencies.ToListAsync();

        var creditCardToCreate = model.Adapt<CreditCard>();

        _context.CreditCards.Add(creditCardToCreate);

        await _context.SaveChangesAsync();

        var creditCardDTO = creditCardToCreate.Adapt<CreditCardDTO>();

        return creditCardDTO;

    }

    public async Task<bool> Delete(int id)
    {
        var creditcard = await _context.CreditCards.FindAsync(id);

        if (creditcard is null) throw new Exception("Credit Card not found");

        _context.CreditCards.Remove(creditcard);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<CreditCardDTO> GetById(int id)
    {
        var customers = await _context.Customers.ToListAsync();
        var banks = await _context.Banks.ToListAsync();
        var currencies = await _context.Currencies.ToListAsync();


        var creditcard = await _context.CreditCards.FindAsync(id);

        if (creditcard is null) throw new Exception("Credit Card not found");

        var creditcardDTO = creditcard.Adapt<CreditCardDTO>();

        return creditcardDTO;
    }

    public async Task<List<CreditCardDTO>> GetFiltered(FilterCreditCardModel filter)
    {
        var query = _context.CreditCards
            .Include(c => c.Currency)
            .Include(c => c.Customer)
            .ThenInclude(c => c.Bank)
            .AsQueryable();

        if (filter.CustomerId is not null)
        {
            query = query.Where(x =>
                (x.CustomerId).Equals(filter.CustomerId));
        }

        if (filter.Designation is not null)
        {
            string normalizedFilterName = filter.Designation.ToLower();
            query = query.Where(x =>
                (x.Designation).ToLower().Equals(normalizedFilterName));
        }

        if (filter.Id is not null)
        {
            query = query.Where(x =>
                 x.Id.Equals(filter.Id));
        }

        var result = await query.ToListAsync();
        var creditcardDTO = result.Adapt<List<CreditCardDTO>>();
        return creditcardDTO;
    }

    public async Task<CreditCardDTO> Update(UpdateCreditCardModel model)
    {
        var customers = await _context.Customers.ToListAsync();
        var banks = await _context.Banks.ToListAsync();
        var currencies = await _context.Currencies.ToListAsync();

        var creditcard = await _context.CreditCards.FindAsync(model.Id);

        if (creditcard is null) throw new Exception("Credit Card was not found");

        model.Adapt(creditcard);

        _context.CreditCards.Update(creditcard);

        await _context.SaveChangesAsync();

        var creditcardDTO = creditcard.Adapt<CreditCardDTO>();

        return creditcardDTO;
    }

    public async Task<(bool customerExists, bool currencyExists)> VerifyCustomerAndCurrencyExist(int customerId, int currencyId)
    {
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == currencyId);

        return (customerExists, currencyExists);
    }
}
