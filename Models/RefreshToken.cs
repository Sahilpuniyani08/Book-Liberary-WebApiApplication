namespace LiberaryApp.Models
{
    public class RefreshToken
    {
        public int id { get; set; } 
        public int UsreId { get; set; }
        public string token { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
