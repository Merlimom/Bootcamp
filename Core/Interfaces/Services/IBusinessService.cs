using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services;

public interface IBusinessService
{
    Task<BusinessDTO> Add(CreateBusinessModel model);
    Task<BusinessDTO> Update(UpdateBusinessModel model);
    Task<BusinessDTO> GetById(int id);
    Task<bool> Delete(int id);

}
