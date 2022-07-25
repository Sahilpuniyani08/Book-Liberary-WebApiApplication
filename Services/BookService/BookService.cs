using AutoMapper;
using LiberaryApp.Data;
using LiberaryApp.Dtos;
using LiberaryApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LiberaryApp.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookService(DataContext context , IMapper mapper ,IHttpContextAccessor httpContextAccessor)
        {
           _context = context;
           _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        /*
        // add new book 
        public async  Task<ServiceResponse<Book>> AddBook(Book newBook)
        {
            ServiceResponse<Book> response = new ServiceResponse<Book>();

            Book B =  _context.Books.FirstOrDefault(x => x.Name == newBook.Name);

            if (B != null)
            {
                response.Success = false;
                response.Message = "the book is already in the liberary!";
                return response;
            }
            else
            {
               await  _context.Books.AddAsync(newBook);
                await _context.SaveChangesAsync();
                response.Data = newBook;
                response.Success = true;
                return response;

            }
        }
         */




        private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        //get all books
        public async Task<ServiceResponse<UsersOfBook>> GetBooks(string name )

        {
            ServiceResponse<UsersOfBook> response = new ServiceResponse<UsersOfBook>();
            if (GetUserRole() == "Student")
            {
                UsersOfBook c = new UsersOfBook();
                c.Name = "You cannot see the Liberary Book List ";
                c.Subject =string.Empty;
                c.Author =string.Empty;
                c.SrNumber =0;
                response.Data = c;
                response.Success = false;
               
                return response;
            }

            Book book = await _context.Books.Include(u => u.Users).FirstOrDefaultAsync(c=> c.Name ==name); 
            
            if(book == null)
            {
                response.Success = false;
                response.Message = "book is not accessed!";
                return response;
            }
            



            response.Data = _mapper.Map<UsersOfBook>(book) ;
            response.Success = true;
                return response;
        }

    }
}
