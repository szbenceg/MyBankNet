using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBank.Persistence.Dao;

namespace MyBank.Persistence.Services
{
    public class CustomerContext : IdentityDbContext<Customer, IdentityRole<int>, int>
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Account>().ToTable("Account");
            builder.Entity<Customer>().ToTable("Customer");
            builder.Entity<Employee>().ToTable("Employee");
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

    }
}
