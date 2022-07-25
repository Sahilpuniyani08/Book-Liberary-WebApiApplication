using LiberaryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LiberaryApp.Data
{
    public class DataContext :DbContext
    {
        public DataContext( DbContextOptions<DataContext> options ) :base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
