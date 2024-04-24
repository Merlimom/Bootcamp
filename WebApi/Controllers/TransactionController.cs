using Core.Interfaces.Services;
using Core.Request;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly IAccountTransactionService _accountTransactionService;

        public TransactionController(IAccountTransactionService accountTransactionService)
        {
            _accountTransactionService = accountTransactionService;
        }

        [HttpGet("filtered")]
        public async Task <IActionResult> GetFilteredTransactions([FromQuery] FilterTransactionModel filters)
        {
            var transactions = await _accountTransactionService.GetFilteredAccountTransactions(filters);
            return Ok(transactions);
        }
    }
}
