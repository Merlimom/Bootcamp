using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<List<PaymentDTO>> GetAll();
    Task<PaymentDTO> Add(CreatePaymentModel model);
    Task<bool> VerifyServiceExists(int productId);
    Task<bool> IsSufficientBalance(int sourceAccountId, decimal amount);
    Task<bool> DoesAccountExist(int accountId);

}