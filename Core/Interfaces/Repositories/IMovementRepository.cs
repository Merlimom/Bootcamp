using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IMovementRepository
{
    Task<MovementDTO>Add(CreateMovementModel model);
    Task<List<MovementDTO>> GetAll();


}
