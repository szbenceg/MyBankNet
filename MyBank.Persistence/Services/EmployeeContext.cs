using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBank.Persistence.Dao;

namespace MyBank.Persistence.Services
{
    public class EmployeeContext : IdentityDbContext<Employee, IdentityRole<int>, int>
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Account>().ToTable("Account");
            builder.Entity<Employee>().ToTable("Employee");
            builder.Entity<Customer>().ToTable("Customer");
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

    }
}
