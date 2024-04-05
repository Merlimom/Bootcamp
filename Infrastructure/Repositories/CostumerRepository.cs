using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Core.Requests;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly BootcampContext _context;

    public CustomerRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<CustomerDTO> Add(CreateCustomerModel model)
    {
        var customerToCreate = new Customer
        {
            Name = model.Name,
            Lastname = model.Lastname,
            DocumentNumber = model.DocumentNumber,
            Address = model.Address,
            Mail = model.Mail,  
            Phone = model.Phone,
            CustomerStatus = (CustomerStatus)Enum.Parse(typeof(CustomerStatus), model.CustomerStatus),
            Birth = model.Birth,

        };
        _context.Add(customerToCreate);

        await _context.SaveChangesAsync();

        var customerDTO = new CustomerDTO
        {
            Id = customerToCreate.Id,
            Name = customerToCreate.Name,
            Address = customerToCreate.Address,
            Mail = customerToCreate.Mail,
            Phone = customerToCreate.Phone,
            CustomerStatus = nameof(CustomerStatus),
            Birth = customerToCreate.Birth,
        };

        return customerDTO;
    }


    public async Task<bool> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer is null) throw new Exception("Customer not found");

        _context.Customers.Remove(customer);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<CustomerDTO>> GetAll()
    {
        var customers = await _context.Customers.ToListAsync();

        var customersDTO = customers.Select(customer => new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Lastname = customer.Lastname,
            DocumentNumber = customer.DocumentNumber,
            Address = customer.Address,
            Mail = customer.Mail,
            Phone = customer.Phone,
            CustomerStatus = nameof(CustomerStatus),
            Birth = customer.Birth,

        }).ToList();

        return customersDTO;
    }

    public async Task<CustomerDTO> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CustomerDTO>> GetFiltered(FilterCustomersModel filter)
    {
        var query = _context.Customers
            .Include(c => c.Bank)
            .AsQueryable();

        if (filter.BirthYearFrom is not null)
        {
            query = query.Where(x =>
                x.Birth != null &&
                x.Birth.Value.Year >= filter.BirthYearFrom);
        }

        if (filter.BirthYearTo is not null)
        {
            query = query.Where(x =>
                x.Birth != null &&
                x.Birth.Value.Year <= filter.BirthYearTo);

        }

        if (filter.FullName is not null)
        {
            query = query.Where(x =>
                (x.Name + " " + x.Lastname).Equals(filter.FullName));
        }


        if (filter.DocumentNumber is not null)
        {
            query = query.Where(x =>
                 x.DocumentNumber != null &&
                (x.DocumentNumber).Equals(filter.DocumentNumber));
        }

        if (filter.Mail is not null)
        {
            query = query.Where(x =>
                 x.Mail != null &&
                (x.Mail).Equals(filter.Mail));
        }

        if (filter.BankId is not null)
        {
            query = query.Where(x =>
                (x.BankId).Equals(filter.BankId));
        }

        var result = await query.ToListAsync();

        return result.Select(x => new CustomerDTO
        {
            Id = x.Id,
            Name = x.Name,
            Lastname = x.Lastname,
            DocumentNumber = x.DocumentNumber,
            Address = x.Address,
            Mail = x.Mail,
            Phone = x.Phone,
            CustomerStatus = nameof(x.CustomerStatus),
            Birth = x.Birth,
            Bank = new BankDTO
            {
                Id = x.Bank.Id,
                Name = x.Bank.Name,
                Phone = x.Bank.Phone,
                Mail = x.Bank.Mail,
                Address = x.Bank.Address
            }
        }).ToList();
      

    }

    public Task<bool> NameIsAlreadyTaken(string name)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDTO> Update(UpdateCustomerModel model)
    {
        throw new NotImplementedException();
    }
}