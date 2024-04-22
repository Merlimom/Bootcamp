using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Core.Requests;
using Infrastructure.Repositories;

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

    public async Task<bool> Delete(int id)
    {
        return await _customerRepository.Delete(id);

    }

    public async Task<CustomerDTO> GetById(int id)
    {
        return await _customerRepository.GetById(id);
    }

    public async Task<List<CustomerDTO>> GetFiltered(FilterCustomersModel filter)
    {
        return await _customerRepository.GetFiltered(filter);
    }

    public async Task<CustomerDTO> Update(UpdateCustomerModel model)
    {
        return await _customerRepository.Update(model);
    }
}