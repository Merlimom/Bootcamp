using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IEnterpriseRepository
{
    Task<EnterpriseDTO> Add(CreateEnterpriseModel model);
    Task<EnterpriseDTO> Update(UpdateEnterpriseModel model);
    Task<EnterpriseDTO> GetById(int id);
    Task<bool> Delete(int id);
}
