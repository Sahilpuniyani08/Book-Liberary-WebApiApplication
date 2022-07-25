using LiberaryApp.Dtos;
using LiberaryApp.Models;

namespace LiberaryApp.Services.StudentService
{
    public interface IStudentService
    {

       Task<ServiceResponse<GetStudentBookDto>> GetAllBooks( );
       Task<ServiceResponse<GetStudentBookDto>> AddNewBook(AddStudnetBookDto newBook);
       Task<ServiceResponse<GetStudentBookDto>> DeleteStudentBook(AddStudnetBookDto newBook);
    }
}
