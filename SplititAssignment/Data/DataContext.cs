using Microsoft.EntityFrameworkCore;
using SplititAssignment.Models;

namespace SplititAssignment.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Actor> Actors { get; set; }
    }
}
