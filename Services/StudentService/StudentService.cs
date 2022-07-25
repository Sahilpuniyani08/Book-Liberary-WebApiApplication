using AutoMapper;
using LiberaryApp.Data;
using LiberaryApp.Dtos;
using LiberaryApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LiberaryApp.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccerssor;

        public StudentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccerssor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccerssor = httpContextAccerssor;
        }

       

        private int GetUserId() => int.Parse(_httpContextAccerssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string GetUserRole() => _httpContextAccerssor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        public async Task<ServiceResponse<GetStudentBookDto>> GetAllBooks()
        {

            ServiceResponse<GetStudentBookDto> response = new ServiceResponse<GetStudentBookDto>();
            if(GetUserRole() == "Admin")
            {
                response.Success = false;
                response.Message = "You cannot see the student Book List";
                return response;
            }

               User usr = await _context.Users.Include(b => b.Books).FirstAsync(c=>c.Id == GetUserId());
         

            response.Data =  _mapper.Map<GetStudentBookDto>(usr);
            return response;
        }







        public async Task<ServiceResponse<GetStudentBookDto>> AddNewBook(AddStudnetBookDto newBook)
        {
            ServiceResponse<GetStudentBookDto> response = new ServiceResponse<GetStudentBookDto>();

            if (GetUserRole() == "Admin")
            {
                response.Success = false;
                response.Message = "You cannot see the student Book List";
                return response;
            }
            User usr = await _context.Users.Include(b => b.Books).FirstAsync(c => c.Id == GetUserId());

            foreach (var book in usr.Books)
                {
              //  book.Users = _context.Books.FirstOrDefault(c => c.Id == book.Id).Users;
                   
                        if (book.Name==newBook.Name)
                        {
                            response.Success = false;
                            response.Message = "the book is already in your list";
                            return response;
                           
                        }
                    }
                
               // Book book = await _context.Books.FirstOrDefaultAsync(c => c.Name == newBook.Name);
           
          
                Book bu =  _mapper.Map<Book>(newBook);
              
               
                bu.Users = new List<User>();
                bu.Users.Add(usr);
                usr.Books.Add(bu);
               _context.Books.Add(bu);
                await _context.SaveChangesAsync();


            // List<Book> a = await _context.Books.ToListAsync();
           // User usr = await _context.Users.Include(b => b.Books).FirstAsync(c => c.Id == GetUserId());


            response.Data = _mapper.Map<GetStudentBookDto>(usr);
            return response;



        }

        public async Task<ServiceResponse<GetStudentBookDto>> DeleteStudentBook(AddStudnetBookDto request)
        {
            ServiceResponse<GetStudentBookDto> response = new ServiceResponse<GetStudentBookDto>();

            if (GetUserRole() == "Admin")
            {
                response.Success = false;
                response.Message = "You cannot see the student Book List";
                return response;
            }

            User usr = await _context.Users.Include(b => b.Books).FirstAsync(c => c.Id == GetUserId());

            foreach (var book in usr.Books)
            {
                //  book.Users = _context.Books.FirstOrDefault(c => c.Id == book.Id).Users;

                if (book.Name == request.Name)
                {
                    usr.Books.Remove(book);
                    book.Users.Remove(usr);
                    await _context.SaveChangesAsync();
                    response.Data = _mapper.Map<GetStudentBookDto>(usr);
                    return response;

                }
            }             

response.Success = false;
response.Message = "the book is not in your list";
return response;
        }
    }
}
