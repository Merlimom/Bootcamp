using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services;

public interface IPaymentService
{
    Task<List<PaymentDTO>> GetAll();
    Task<PaymentDTO> Add(CreatePaymentModel model);
}