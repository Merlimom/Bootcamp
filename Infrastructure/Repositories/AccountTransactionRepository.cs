using Core.Models;
using Core.Request;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using Mapster;

namespace Infrastructure.Repositories
{
    public class AccountTransactionRepository : IAccountTransactionRepository
    {
        private readonly BootcampContext _context;

        public AccountTransactionRepository(BootcampContext context)
        {
            _context = context;
        }

        public async Task<List<AccountTransactionsDTO>> GetFilteredAccountTransactions(FilterTransactionModel filters)
        {

            var query = _context.Accounts
                .Include(a => a.Movements)
                .Include(a => a.Deposits)
                .Include(a => a.Withdrawals)
                .Include(a => a.Payments)
                .AsQueryable();



            query = query.Where(a => a.Id == filters.AccountId);

            // Aplicar filtro de Mes/Año
            if (filters.Month.HasValue && filters.Year.HasValue)
            {
                var startDate = new DateTime(filters.Year.Value, filters.Month.Value, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                query = query.Where(a =>
                    a.Movements.Any(m => m.TransferredDateTime >= startDate && m.TransferredDateTime <= endDate) ||
                    a.Deposits.Any(d => d.DepositDateTime >= startDate && d.DepositDateTime <= endDate) ||
                    a.Withdrawals.Any(w => w.WithdrawalDateTime >= startDate && w.WithdrawalDateTime <= endDate)
                );
            }
            else if (filters.Month.HasValue || filters.Year.HasValue)
            {
                throw new Exception("Both month and year must be specified for filtering by Month/Year.");
            }

            // Aplicar filtro de Rango de Fechas
            if (filters.FromDate.HasValue && filters.ToDate.HasValue)
            {
                query = query.Where(a =>
                    a.Movements.Any(m => m.TransferredDateTime >= filters.FromDate && m.TransferredDateTime <= filters.ToDate) ||
                    a.Deposits.Any(d => d.DepositDateTime >= filters.FromDate && d.DepositDateTime <= filters.ToDate) ||
                    a.Withdrawals.Any(w => w.WithdrawalDateTime >= filters.FromDate && w.WithdrawalDateTime <= filters.ToDate)
                  
                );
            }

            // Aplicar filtro de Descripción
            if (!string.IsNullOrWhiteSpace(filters.Description))
            {
                var description = filters.Description.ToLower();
                query = query.Where(a =>
                    a.Movements.Any(m => m.Description.ToLower().Contains(description)) ||
                    a.Payments.Any(p => p.Description.ToLower().Contains(description))
                );
            }

            var result = await query.ToListAsync();

            var accountTransactionsDTOs = result.SelectMany(a => a.Movements.Select(m => m.Adapt<AccountTransactionsDTO>())
                                                                .Concat(a.Deposits.Select(d => d.Adapt<AccountTransactionsDTO>()))
                                                                .Concat(a.Withdrawals.Select(w => w.Adapt<AccountTransactionsDTO>()))
                                                                .Concat(a.Payments.Select(p => p.Adapt<AccountTransactionsDTO>())))
                                               .ToList();

            return accountTransactionsDTOs;

        }
    }
}
