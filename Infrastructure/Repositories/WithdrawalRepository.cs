using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly BootcampContext _context;

    public WithdrawalRepository(BootcampContext context)
    {
        _context = context;
    }
    public async Task<WithdrawalDTO> Add(CreateWithdrawalModel model)
    {
        var withdrawalToCreate = model.Adapt<Withdrawal>();

        await UpdateAccountBalanceForWithdrawal(model.AccountId, model.Amount);


        _context.Withdrawals.Add(withdrawalToCreate);

        var result = await _context.SaveChangesAsync();

        var query = await _context.Withdrawals
            .Include(a => a.Account)
                .ThenInclude(a => a.Customer)
                    .ThenInclude(a => a.Bank)
            .SingleOrDefaultAsync(r => r.Id == withdrawalToCreate.Id);

        var withdrawalDTO = query.Adapt<WithdrawalDTO>();
        return withdrawalDTO;
    }

    public async Task UpdateAccountBalanceForWithdrawal(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account != null)
        {
            if (account.Balance < amount)
            {
                // Si el saldo de la cuenta es menor que el monto de la extracción, lanzar una excepción de negocio
                throw new BusinessLogicException("Insufficient balance in the account to process this withdrawal.");
            }

            // Actualizar el saldo de la cuenta
            account.Balance -= amount;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
    }


    public async Task<List<WithdrawalDTO>> GetAll()
    {
        var query = _context.Withdrawals
                       .Include(a => a.Account)
                       .AsQueryable();

        var deposits = await _context.Withdrawals.ToListAsync();

        var withdrawalDTO = deposits.Adapt<List<WithdrawalDTO>>();

        return withdrawalDTO;
    }

    public async Task<bool> DoesAccountExist(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account != null;
    }

    public async Task<bool> DoesBankExist(int bankId)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        return bank != null;
    }

}