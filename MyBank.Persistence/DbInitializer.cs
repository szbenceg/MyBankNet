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


        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            _customerContext = serviceProvider.GetRequiredService<Services.CustomerContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();
            _employeeManager = serviceProvider.GetRequiredService<UserManager<Employee>>();

            _customerContext.Database.Migrate();

            Employee emp = _employeeManager.FindByNameAsync("manager").Result;
            if (emp == null)
            { 
                var employee = new Employee
                {
                    Name = "manager",
                    UserName = "manager",
                };

                try
                {
                    var result = await _employeeManager.CreateAsync(employee, "Password1");
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
