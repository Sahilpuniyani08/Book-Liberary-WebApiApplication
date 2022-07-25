using LiberaryApp.Dtos;
using LiberaryApp.Models;

namespace LiberaryApp.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResponse<UsersOfBook>> GetBooks(string name);
        //Task<ServiceResponse<Book>> AddBook(Book newBook);  
    }
}
