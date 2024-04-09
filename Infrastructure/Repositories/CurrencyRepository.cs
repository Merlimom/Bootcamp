using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly BootcampContext _context;

    public CurrencyRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<CurrencyDTO> Add(CreateCurrencyModel model)
    {
        //var bank = await _context.Banks.FindAsync(model.BankId);

        var currencyToCreate = model.Adapt<Currency>();

        _context.Add(currencyToCreate);
        await _context.SaveChangesAsync();

        var currencyDTO = currencyToCreate.Adapt<CurrencyDTO>();

        return currencyDTO;
    }

    public async Task<bool> Delete(int id)
    {
        var currency = await _context.Currencies.FindAsync(id);

        if (currency is null) throw new Exception("Currency not found");

        _context.Currencies.Remove(currency);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<CurrencyDTO>> GetAll()
    {
        //var banks = await _context.Banks.ToListAsync();

        var currencies = await _context.Currencies.ToListAsync();

        var currencyDTO = currencies.Adapt<List<CurrencyDTO>>();

        return currencyDTO;
    }

    public async Task<CurrencyDTO> GetById(int id)
    {
        //var banks = await _context.Banks.ToListAsync();

        var currency = await _context.Currencies.FindAsync(id);

        if (currency is null) throw new Exception("Currency not found");

        var currencyDTO = currency.Adapt<CurrencyDTO>();

        return currencyDTO;
    }

    public async Task<List<CurrencyDTO>> GetFiltered(FilterCurrencyModel filter)
    {
        var query = _context.Currencies.AsQueryable();

        if (filter.Name is not null)
        {
            //Aplicar el filtro por nombre ignorando mayúsculas y minúsculas
            string normalizedFilterName = filter.Name.ToLower();
            query = query.Where(x =>
                (x.Name).ToLower().Equals(normalizedFilterName));

        }

        var result = await query.ToListAsync();
        var currencyDTO = result.Adapt<List<CurrencyDTO>>();
        return currencyDTO;
    }


    public async Task<CurrencyDTO> Update(UpdateCurrencyModel model)
    {
        //var bank = await _context.Banks.FindAsync(model.BankId);

        var currency = await _context.Currencies.FindAsync(model.Id);

        if (currency is null) throw new Exception("Currency was not found");

        model.Adapt(currency);

        _context.Currencies.Update(currency);

        await _context.SaveChangesAsync();

        var currencyDTO = currency.Adapt<CurrencyDTO>();

        return currencyDTO;
    }
}
