using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookLibraryManagementSystem.Data;
using BookLibraryManagementSystem.Models;

namespace BookLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionLogsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TransactionLogsController(LibraryContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<TransactionLog>> GetTransactionLogs()
        //{
        //    return _context.TransactionLogs.ToList();
        //}
    }
}
