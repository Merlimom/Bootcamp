using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;

namespace Infrastructure.Repositories;

public class MovementRepository : IMovementRepository
{
    public Task<MovementDTO> Add(CreateMovementModel model)
    {
        throw new NotImplementedException();
    }

    public Task<List<MovementDTO>> GetAll()
    {
        throw new NotImplementedException();
    }
}
