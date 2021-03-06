using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Services;

namespace MyBank.Persistence.Model
{
    public class DbInitializer
    {
        private static CustomerContext _customerContext = null!;
        private static UserManager<Customer> _userManager = null!;
        private static UserManager<Employee> _employeeManager = null!;
        private static RoleManager<IdentityRole<int>> _roleManager = null!;


        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            _customerContext = serviceProvider.GetRequiredService<Services.CustomerContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();
            _employeeManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            _customerContext.Database.Migrate();

            Employee emp = _employeeManager.FindByNameAsync("manager").Result;
            if (emp == null)
            { 
                var employee = new Employee
                {
                    Name = "manager",
                    UserName = "manager"
                };

                var adminRole = new IdentityRole<int>("administrator");

                try
                {
                    var result = await _employeeManager.CreateAsync(employee, "Password1");
                    var result2 = _roleManager.CreateAsync(adminRole).Result;
                    var result3 = _employeeManager.AddToRoleAsync(employee, adminRole.Name).Result;
                    if (!result.Succeeded)
                    {

                    }
                }
                catch (Exception ex) 
                {
                    int a = 0;
                }
            }
               
        }
    }
}
