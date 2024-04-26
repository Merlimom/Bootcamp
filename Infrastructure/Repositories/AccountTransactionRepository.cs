using Core.Models;
using Core.Request;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using Mapster;
using Core.Exceptions;

namespace Infrastructure.Repositories;

public class AccountTransactionRepository : IAccountTransactionRepository
{
    private readonly BootcampContext _context;

    public AccountTransactionRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<List<AccountTransactionsDTO>> GetFilteredAccountTransactions(FilterTransactionModel filters)
    {

        var accountExists = await _context.Accounts.AnyAsync(a => a.Id == filters.AccountId);
        if (!accountExists)
        {
            throw new ArgumentException("Invalid Account ID. Account does not exist.");
        }

        if (filters.Month.HasValue && !filters.Year.HasValue)
        {
            throw new ArgumentException("Year must be specified when filtering by month.");
        }


        if (filters.Month < 1 || filters.Month > 12)
        {
            throw new ArgumentException("Month must be a value between 1 and 12.");
        }

        if (filters.Year < 2000)
        {
            throw new ArgumentException("Year must be greater than 2000.");
        }

        var transactionsQuery = _context.Accounts
       .Include(a => a.Movements)
       .Include(a => a.Deposits)
       .Include(a => a.Withdrawals)
       .Include(a => a.Payments)
       .Where(a => a.Id == filters.AccountId);

        if (filters.Month.HasValue && filters.Year.HasValue)
        {
            var month = filters.Month.Value;
            var year = filters.Year.Value;
            transactionsQuery = transactionsQuery.Where(a =>
                a.Movements.Any(m => m.TransferredDateTime.Month == month && m.TransferredDateTime.Year == year) ||
                a.Deposits.Any(d => d.DepositDateTime.Month == month && d.DepositDateTime.Year == year) ||
                a.Withdrawals.Any(w => w.WithdrawalDateTime.Month == month && w.WithdrawalDateTime.Year == year) ||
                a.Payments.Any(p => p.DepositDateTime.Month == month && p.DepositDateTime.Year == year)
            );
        }

        var transactions = await transactionsQuery.ToListAsync();

        var filteredTransactions = transactions.SelectMany(t =>
        {
            var filteredMovements = t.Movements.Where(m =>
                (!filters.Month.HasValue || m.TransferredDateTime.Month == filters.Month.Value) &&
                (!filters.Year.HasValue || m.TransferredDateTime.Year == filters.Year.Value) &&
                (!filters.FromDate.HasValue || m.TransferredDateTime >= filters.FromDate.Value) &&
                (!filters.ToDate.HasValue || m.TransferredDateTime <= filters.ToDate.Value)
            );

            var filteredDeposits = t.Deposits.Where(d =>
                (!filters.Month.HasValue || d.DepositDateTime.Month == filters.Month.Value) &&
                (!filters.Year.HasValue || d.DepositDateTime.Year == filters.Year.Value) &&
                (!filters.FromDate.HasValue || d.DepositDateTime >= filters.FromDate.Value) &&
                (!filters.ToDate.HasValue || d.DepositDateTime <= filters.ToDate.Value)
            );

            var filteredWithdrawals = t.Withdrawals.Where(w =>
                (!filters.Month.HasValue || w.WithdrawalDateTime.Month == filters.Month.Value) &&
                (!filters.Year.HasValue || w.WithdrawalDateTime.Year == filters.Year.Value) &&
                (!filters.FromDate.HasValue || w.WithdrawalDateTime >= filters.FromDate.Value) &&
                (!filters.ToDate.HasValue || w.WithdrawalDateTime <= filters.ToDate.Value)
            );

            var filteredPayments = t.Payments.Where(p =>
                (!filters.Month.HasValue || p.DepositDateTime.Month == filters.Month.Value) &&
                (!filters.Year.HasValue || p.DepositDateTime.Year == filters.Year.Value) &&
                (!filters.FromDate.HasValue || p.DepositDateTime >= filters.FromDate.Value) &&
                (!filters.ToDate.HasValue || p.DepositDateTime <= filters.ToDate.Value)
            );

            if (!string.IsNullOrWhiteSpace(filters.Description))
            {
                var descriptionFilter = filters.Description.ToLower();
                transactionsQuery = transactionsQuery.Where(a =>
                    a.Movements.Any(m => m.Description.ToLower().Contains(descriptionFilter)) ||
                    a.Deposits.Any(d => d.Description.ToLower().Contains(descriptionFilter)) ||
                    a.Withdrawals.Any(w => w.Description.ToLower().Contains(descriptionFilter)) ||
                    a.Payments.Any(p => p.Description.ToLower().Contains(descriptionFilter))
                );
            }

            var combinedResults = new List<AccountTransactionsDTO>();

            if (filteredMovements.Any())
                combinedResults.AddRange(filteredMovements.Select(m => m.Adapt<AccountTransactionsDTO>()));

            if (filteredDeposits.Any())
                combinedResults.AddRange(filteredDeposits.Select(d => d.Adapt<AccountTransactionsDTO>()));

            if (filteredWithdrawals.Any())
                combinedResults.AddRange(filteredWithdrawals.Select(w => w.Adapt<AccountTransactionsDTO>()));

            if (filteredPayments.Any())
                combinedResults.AddRange(filteredPayments.Select(p => p.Adapt<AccountTransactionsDTO>()));

            if (filters.TransactionType != null)
            {
                var transactionTypeLowerCase = filters.TransactionType.ToLowerInvariant();
                combinedResults = combinedResults.Where(t => t.Type.ToLowerInvariant() == transactionTypeLowerCase).ToList();
            }

            return combinedResults.Where(dto =>
                     string.IsNullOrWhiteSpace(filters.Description) ||
                     dto.Description.ToLower().Contains(filters.Description.ToLower())
                 );
        }).ToList();

        if (filteredTransactions.Count == 0)
        {
            throw new BusinessLogicException("No transactions to display.");
        }

        return filteredTransactions;
    }
  }
