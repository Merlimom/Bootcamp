using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services;

public interface IDepositService
{
    Task<List<DepositDTO>> GetAll();
    Task<DepositDTO> Add(CreateDepositModel model);
}
