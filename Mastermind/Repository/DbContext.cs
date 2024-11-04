using Mastermind.Models;
using Microsoft.EntityFrameworkCore;

namespace Mastermind.Repository
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        { }
        public DbSet<Highscores> HighScores { get; set; }
    }
}
