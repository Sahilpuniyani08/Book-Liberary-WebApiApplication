

using System.Text.Json.Serialization;

namespace LiberaryApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName
        {
            get;
            set;
        } = string.Empty;

        public string Role { get; set; }

        [JsonIgnore]

        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        [JsonIgnore]
        public List<Book> Books { get; set; }
    }
}
