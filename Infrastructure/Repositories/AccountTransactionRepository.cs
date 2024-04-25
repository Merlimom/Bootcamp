using Core.Models;
using Core.Request;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using Mapster;

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
        // Imprimir los valores de filters.Month y filters.Year para depuración
        Console.WriteLine($"Month: {filters.Month}, Year: {filters.Year}");


        var accountExists = await _context.Accounts.AnyAsync(a => a.Id == filters.AccountId);
        if (!accountExists)
        {
            throw new ArgumentException("Invalid Account ID. Account does not exist.");
        }

        if (filters.Month.HasValue && !filters.Year.HasValue)
        {
            throw new ArgumentException("Year must be specified when filtering by month.");
        }


        // Verificar que los valores de mes y año estén dentro de los rangos permitidos
        if (filters.Month < 1 || filters.Month > 12)
        {
            throw new ArgumentException("Month must be a value between 1 and 12.");
        }

        if (filters.Year < 2000)
        {
            throw new ArgumentException("Year must be greater than 2000.");
        }

        var query = _context.Accounts
            .Include(a => a.Movements)
            .Include(a => a.Deposits)
            .Include(a => a.Withdrawals)
            .Include(a => a.Payments)
            .AsQueryable();


        query = query.Where(a => a.Id == filters.AccountId);


        Console.WriteLine("Consulta SQL antes de aplicar filtros:");
        Console.WriteLine(query.ToQueryString());

        if (filters.Month.HasValue && filters.Year.HasValue)
        {
            var month = filters.Month.Value;
            var year = filters.Year.Value;

            Console.WriteLine($"Mes: {month}, Año: {year}");

            query = query.Where(a => a.Id == filters.AccountId &&
                (a.Movements.Any(m =>
                    m.TransferredDateTime.Year == year &&
                    m.TransferredDateTime.Month == month
                ) ||
                a.Deposits.Any(d =>
                    d.DepositDateTime.Year == year &&
                    d.DepositDateTime.Month == month
                ) ||
                a.Withdrawals.Any(w =>
                    w.WithdrawalDateTime.Year == year &&
                    w.WithdrawalDateTime.Month == month
                ) ||
                a.Payments.Any(p =>
                    p.DepositDateTime.Year == year &&
                    p.DepositDateTime.Month == month
                ))
            );
        }

        Console.WriteLine("Consulta SQL después de aplicar filtro por mes y año:");
        Console.WriteLine(query.ToQueryString());

        if (filters.FromDate.HasValue)
        {
            var fromDate = filters.FromDate.Value;
            query = query.Where(a =>
                a.Movements.Any(m => m.TransferredDateTime >= fromDate) ||
                a.Deposits.Where(d => d.DepositDateTime >= fromDate).Any() ||
                a.Withdrawals.Where(w => w.WithdrawalDateTime >= fromDate).Any() ||
                a.Payments.Where(p => p.DepositDateTime >= fromDate).Any()
            );
        }

        // Aplicar filtro de fecha de fin si está especificado
        if (filters.ToDate.HasValue)
        {
            var toDate = filters.ToDate.Value;
            query = query.Where(a =>
                a.Movements.Any(m => m.TransferredDateTime <= toDate) ||
                a.Deposits.Where(d => d.DepositDateTime <= toDate).Any() ||
                a.Withdrawals.Where(w => w.WithdrawalDateTime <= toDate).Any() ||
                a.Payments.Where(p => p.DepositDateTime <= toDate).Any()
            );
        }

        // Aplicar filtro de Descripción
        if (!string.IsNullOrWhiteSpace(filters.Description))
        {
            var description = filters.Description.ToLower();
            query = query.Where(a =>
                a.Movements.Any(m => m.Description.ToLower().Contains(description)) ||
                a.Deposits.Where(d => d.Description.ToLower().Contains(description)).Any() ||
                a.Withdrawals.Where(w => w.Description.ToLower().Contains(description)).Any() ||
                a.Payments.Where(p => p.Description.ToLower().Contains(description)).Any()
            );
        }



        if (!string.IsNullOrWhiteSpace(filters.MovementType))
        {
            switch (filters.MovementType.ToLower())
            {
                case "Movements":
                    query = query.Where(a => a.Movements.Any());
                    break;
                case "Deposits":
                    query = query.Where(a => a.Deposits.Any());
                    break;
                case "Withdrawals":
                    query = query.Where(a => a.Withdrawals.Any());
                    break;
                case "Payments":
                    query = query.Where(a => a.Payments.Any());
                    break;
                default:
                    break;
            }
        }


        var result = await query.ToListAsync();

        var accountTransactionsDTOs = result.SelectMany(a => a.Movements.Select(m => m.Adapt<AccountTransactionsDTO>())
                                                            .Concat(a.Deposits.Select(d => d.Adapt<AccountTransactionsDTO>()))
                                                            .Concat(a.Withdrawals.Select(w => w.Adapt<AccountTransactionsDTO>()))
                                                            .Concat(a.Payments.Select(p => p.Adapt<AccountTransactionsDTO>())))
                                           .ToList();

        foreach (var transaction in accountTransactionsDTOs)
        {
            Console.WriteLine($"Transaction Id: {transaction.Id}, Type: {transaction.Type}, Description: {transaction.Description}, Amount: {transaction.Amount}, Date: {transaction.TransferredDateTime}");
        }


        return accountTransactionsDTOs;

    }

}


