using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services; 

public interface IUserRequestService
{
    Task<List<UserRequestDTO>> GetAll();
    Task<UserRequestDTO> Add(CreateUserRequestModel model);

}
