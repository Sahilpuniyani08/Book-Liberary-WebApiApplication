using LiberaryApp.Models;

namespace LiberaryApp.Dtos
{
    public class UsersOfBook
    {

        public int Id { get; set; } = 0;
        public string Name { get; set; } = "rd sharma";
        public string Subject { get; set; } = "sciene";
        public string Author { get; set; } = "sahil";
        public int SrNumber { get; set; } = 0;
      
        public List<User>? Users { get; set; }
    }
}
