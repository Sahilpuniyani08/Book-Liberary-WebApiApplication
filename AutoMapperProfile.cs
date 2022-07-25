using AutoMapper;
using LiberaryApp.Dtos;
using LiberaryApp.Models;

namespace LiberaryApp
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, GetStudentBookDto>();
            CreateMap<AddStudnetBookDto, Book > ();
            CreateMap<User, GetStudentBookDto> ();
            CreateMap<Book, UsersOfBook> ();
        }
    }
}
