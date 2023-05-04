using Microsoft.EntityFrameworkCore;// import EF
using finalLab.Models;



namespace finalLab.Database
{
    public class DataDbContext : DbContext
    {
        // Constructor Method
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }
        // Take Employees
        public DbSet<Employees> Employees { get; set; }

        // Tale Positions
        public DbSet<Positions> Positions{ get; set; }

    }
}
