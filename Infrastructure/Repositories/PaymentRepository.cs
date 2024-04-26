
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly BootcampContext _context;

    public PaymentRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<PaymentDTO> Add(CreatePaymentModel model)
    {
        var paymentToCreate = model.Adapt<Payment>();

        await UpdateAccountBalance(model.AccountId, model.Amount);


        _context.Payments.Add(paymentToCreate);

        var result = await _context.SaveChangesAsync();

        var query = await _context.Payments
            .Include(a => a.Service)
            .Include(a => a.Account)
                .ThenInclude(a => a.Customer)
            .SingleOrDefaultAsync(r => r.Id == paymentToCreate.Id);

        var paymentDTO = query.Adapt<PaymentDTO>();
        return paymentDTO;
    }

    public async Task<List<PaymentDTO>> GetAll()
    {

        var query = _context.Payments
             .Include(a => a.Account)
             .ThenInclude(a => a.Customer)
             .Include(a => a.Account)
             .Include(a => a.Service)
             .AsQueryable();

        var payments = await _context.Payments.ToListAsync();

        var paymentsDTO = payments.Adapt<List<PaymentDTO>>();

        return paymentsDTO;
    }

    public async Task UpdateAccountBalance(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account != null)
        {
            account.Balance -= amount; 
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> VerifyServiceExists(int serviceId)
    {
        var serviceExists = await _context.Services.AnyAsync(c => c.Id == serviceId);
        return (serviceExists);

    }

    public async Task<bool> IsSufficientBalance(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FindAsync(accountId);

        return account!.Balance >= amount;
    }

    public async Task<bool> DoesAccountExist(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account != null;
    }

}
