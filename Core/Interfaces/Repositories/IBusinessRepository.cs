using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IBusinessRepository
{
    Task<BusinessDTO> Add(CreateBusinessModel model);
    Task<BusinessDTO> Update(UpdateBusinessModel model);
    Task<BusinessDTO> GetById(int id);
    Task<bool> Delete(int id);
}
