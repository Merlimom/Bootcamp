using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRequestRepository : IUserRequestRepository
{
    private readonly BootcampContext _context;

    public UserRequestRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<UserRequestDTO> Add(CreateUserRequestModel model)
    {
        //verificar esto 
        //var result = await query.ToListAsync();

        var userRequestToCreate = model.Adapt<UserRequest>();

        _context.UserRequests.Add(userRequestToCreate);

        await _context.SaveChangesAsync();


        var query = await _context.UserRequests
         .Include(a => a.Currency)
         .Include(a => a.Product)
         .Include(a => a.Customer)
         .ThenInclude(a => a.Bank)
         .SingleOrDefaultAsync(r => r.Id == userRequestToCreate.Id);

        var userRequestDTO = userRequestToCreate.Adapt<UserRequestDTO>();

        return userRequestDTO;
    }

    public async Task<List<UserRequestDTO>> GetAll()
    {

        var query = _context.UserRequests
                  .Include(a => a.Currency)
                  .Include(a => a.Customer)
                  .ThenInclude(a => a.Bank)
                  .Include(a => a.Product)
                  .AsQueryable();

        var result = await query.ToListAsync();

        var userRequests = await _context.UserRequests.ToListAsync();

        var userRequestsDTO = userRequests.Adapt<List<UserRequestDTO>>();

        return userRequestsDTO;
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
