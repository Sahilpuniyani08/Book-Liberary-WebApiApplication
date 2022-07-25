namespace LiberaryApp.Dtos
{
    public class UserRegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
