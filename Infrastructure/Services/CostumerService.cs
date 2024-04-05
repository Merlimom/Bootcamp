using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Core.Requests;

namespace Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDTO> Add(CreateCustomerModel model)
    {
        bool nameIsInUse = await _customerRepository.NameIsAlreadyTaken(model.Name);

        if (nameIsInUse)
        {
            throw new Exception("Name is already in use");
        }

        return await _customerRepository.Add(model);
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<CustomerDTO>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDTO> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CustomerDTO>> GetFiltered(FilterCustomersModel filter)
    {
        return await _customerRepository.GetFiltered(filter);
    }

    public Task<CustomerDTO> Update(UpdateCustomerModel model)
    {
        throw new NotImplementedException();
    }
}