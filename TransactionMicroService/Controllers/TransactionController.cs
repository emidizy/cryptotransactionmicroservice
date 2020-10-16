using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Application.Interfaces;
using Domain.DTOs.GetTransactionReq;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransactionMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionSvc;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionSvc = transactionService;
        }

        [HttpPost("all/retrieve")]
        public IActionResult GetAllTransactions()
        {
            var response = _transactionSvc.RetrieveAllTransactions();
            return Ok(response);
        }

        [HttpPost("client/retrieve")]
        public IActionResult GetClientTransactions([FromBody] GetTransactionReq req)
        {
            var response = _transactionSvc.GetClientTransactions(req);
            return Ok(response);
        }
    }
}
