using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class BusinessService : IBusinessService
{
    private readonly IBusinessRepository _businessRepository;

    public BusinessService(IBusinessRepository businessRepository)
    {
        _businessRepository = businessRepository;
    }

    public async Task<BusinessDTO> Add(CreateBusinessModel model)
    {
        return await _businessRepository.Add(model);
    }

    public async Task<bool> Delete(int id)
    {
        return await _businessRepository.Delete(id);
    }

    public async Task<BusinessDTO> GetById(int id)
    {
        return await _businessRepository.GetById(id);
    }

    public async Task<BusinessDTO> Update(UpdateBusinessModel model)
    {
        return await _businessRepository.Update(model);
    }
}
