using LiberaryApp.Models;
using LiberaryApp.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiberaryApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookContrller : ControllerBase
    {
        private readonly IBookService _book;


        public BookContrller(IBookService book)
        {
            _book = book;
        }

        [HttpGet("Users of a Book")]
         
        public async Task<IActionResult> GetAllBooks(string name)
        {
            return Ok ( await _book.GetBooks(name));
        }
        


    }
}
