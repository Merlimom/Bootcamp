using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services;

public interface IMovementService
{
    Task<MovementDTO> Add(CreateMovementModel model);
    Task<List<MovementDTO>> GetAll();
}
