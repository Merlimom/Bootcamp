
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }


    public async Task<PaymentDTO> Add(CreatePaymentModel model)
    {
        var serviceExists = await _paymentRepository.VerifyServiceExists(model.ServiceId);
        if (!serviceExists)
        {
            throw new BusinessLogicException($"Service {model.ServiceId} does not exist.");
        }

        var sufficientBalance = await _paymentRepository.IsSufficientBalance(model.AccountId, model.Amount);
        if (!sufficientBalance)
        {
            throw new BusinessLogicException($"Insufficient balance in account {model.AccountId}.");
        }

        return await _paymentRepository.Add(model);
    }


    public async Task<List<PaymentDTO>> GetAll()
    {
        return await _paymentRepository.GetAll();
    }
}
