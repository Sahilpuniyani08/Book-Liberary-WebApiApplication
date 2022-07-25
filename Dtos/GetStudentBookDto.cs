using LiberaryApp.Models;

namespace LiberaryApp.Dtos
{
    public class GetStudentBookDto
    {
        public int Id { get; set; }
        public string UserName { get; set;}
        public List<Book> Books { get; set; }



    }
}
