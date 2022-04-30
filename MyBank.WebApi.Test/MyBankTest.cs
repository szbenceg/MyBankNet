using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBank.Persistence.Dao;
using MyBank.Persistence.Dto;
using MyBank.Persistence.Services;
using MyBank.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyBank.WebApi.Test
{
    public class MyBankTest : IDisposable
    {
        private readonly EmployeeContext _context;
        private readonly EmployeeServiceImp _employeeServiceImp;

        private readonly List<CustomerDto> _customerDtos;
        private readonly List<TransactionHistoryDto> _transactionHistoryDtos;

        //Minden tesz eset elõtt lefut
        public MyBankTest()
        {
            var options = new DbContextOptionsBuilder<EmployeeContext>()
            .UseInMemoryDatabase("MyBankTest")
            .Options;

            _context = new EmployeeContext(options);
            _context.Database.EnsureCreated();

            var customers = new List<Customer>
            {
                new Customer
                {
                    Name = "Test Elek",
                    UserName = "testelek",
                    PinCode = "123456",
                    Accounts = new List<Account>
                    {
                        new Account {
                            Id = 1,
                            AccountNumber = "1111-1111-1111-1111",
                            CustomerId = 1,
                            Balance = 1000000,
                            IsLocked = false,
                            Created = System.DateTime.Now
                        },
                         new Account {
                            Id = 2,
                            AccountNumber = "2222-2222-2222-2222",
                            CustomerId = 1,
                            Balance = 500000,
                            IsLocked = false,
                            Created = System.DateTime.Now
                        }
                    }
                },
                new Customer
                {
                    Name = "Mek Egek",
                    UserName = "mekegek",
                    PinCode = "123456",
                    Accounts = new List<Account>
                    {
                        new Account {
                            Id = 3,
                            AccountNumber = "3333-3333-3333-3333",
                            CustomerId = 2,
                            Balance = 1000000,
                            IsLocked = false,
                            Created = System.DateTime.Now
                        },
                         new Account {
                            Id = 4,
                            AccountNumber = "4444-4444-4444-4444",
                            CustomerId = 2,
                            Balance = 500000,
                            IsLocked = false,
                            Created = System.DateTime.Now
                        }
                    }
                },
                new Customer
                {
                    Name = "Ures",
                    UserName = "ures",
                    PinCode = "123456",
                    Accounts = new List<Account>
                    {
                        new Account {
                            Id = 5,
                            AccountNumber = "5555-5555-5555-5555",
                            CustomerId = 3,
                            Balance = 1000000,
                            IsLocked = false,
                            Created = System.DateTime.Now
                        }
                    }
                }
            };

            var emloyees = new List<Employee>
            {
                new Employee
                {
                    Name = "admin",
                    UserName = "admin"
                }
            };

            //var accounts = new List<Account>
            //{
            //    new Account {
            //        Id = 1,
            //        AccountNumber = "1111-1111-1111-1111",
            //        CustomerId = 1,
            //        Balance = 1000000,
            //        IsLocked = false,
            //        Created = System.DateTime.Now
            //    },
            //    new Account {
            //        Id = 2,
            //        AccountNumber = "2222-2222-2222-2222",
            //        CustomerId = 1,
            //        Balance = 500000,
            //        IsLocked = false,
            //        Created = System.DateTime.Now
            //    },
            //    new Account {
            //        Id = 3,
            //        AccountNumber = "3333-3333-3333-3333",
            //        CustomerId = 2,
            //        Balance = 1000000,
            //        IsLocked = false,
            //        Created = System.DateTime.Now
            //    },
            //    new Account {
            //        Id = 4,
            //        AccountNumber = "4444-4444-4444-4444",
            //        CustomerId = 2,
            //        Balance = 500000,
            //        IsLocked = false,
            //        Created = System.DateTime.Now
            //    }
            //};

            var transactions = new List<Transaction>
            { 
                new Transaction
                { 
                    Type = TransactionType.Transfer,
                    SourceAccountId = 1,
                    DestinationAccountId = 3,
                    BenificaryName = "Mek Egek",
                    TransactionExecutionDate = System.DateTime.Now,
                    TransactionTotal = 100000,
                    Message = "Proba tranzakció"
                },
                new Transaction
                {
                 Type = TransactionType.Transfer,
                    SourceAccountId = 1,
                    DestinationAccountId = 4,
                    BenificaryName = "Mek Egek",
                    TransactionExecutionDate = System.DateTime.Now,
                    TransactionTotal = 200000,
                    Message = "Proba tranzakció"
                },
                new Transaction
                {
                    Type = TransactionType.Kivet,
                    SourceAccountId = 1,
                    DestinationAccountId = 1,
                    BenificaryName = "Test Elek",
                    TransactionExecutionDate = System.DateTime.Now,
                    TransactionTotal = 250000,
                    Message = "Proba kivet"
                },
                new Transaction
                {
                    Type = TransactionType.Betet,
                    SourceAccountId = 2,
                    DestinationAccountId = 2,
                    BenificaryName = "Test Elek",
                    TransactionExecutionDate = System.DateTime.Now,
                    TransactionTotal = 300000,
                    Message = "Proba betet"
                },
            };

            _context.Customers.AddRange(customers);
            _context.Employees.AddRange(emloyees);
            _context.Transactions.AddRange(transactions);

            _context.SaveChanges();

            _customerDtos = customers.Select(customer => new CustomerDto
            {
                    Id = customer.Id,
                    Accounts = customer.Accounts.Select(account => (AccountDto)account).ToList(),
                    Name = customer.Name
            }).ToList();

            _transactionHistoryDtos = transactions.Select(transaction => new TransactionHistoryDto
            {
                TransactionType = transaction.Type.ToString(),
                BenificaryName = transaction.BenificaryName,
                SourceAccountNumber = _context.Accounts.First(account => account.Id == transaction.SourceAccountId).AccountNumber,
                DestinationAccountNumber = _context.Accounts.First(account => account.Id == transaction.DestinationAccountId).AccountNumber,
                Message = transaction.Message,
                TransactionTotal = transaction.TransactionTotal,
                ExecutionDate = transaction.TransactionExecutionDate
            }).ToList();

            _employeeServiceImp = new EmployeeServiceImp(null, _context);

        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Fact]
        public void TestCustomerController()
        {
            var controller = new CustomerController(_employeeServiceImp);
            ActionResult<IEnumerable<CustomerDto>> customers = controller.GetCustomers();

            OkObjectResult objectResult = Assert.IsType<OkObjectResult>(customers.Result);
            List<CustomerDto> actual = ((IEnumerable<CustomerDto>)objectResult.Value).ToList();
            var model = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(actual);
            Assert.Equal(_customerDtos, model);

        }

        [Fact]
        public void TestTransacionsCustomerOne()
        {
            var controller = new CustomerController(_employeeServiceImp);
            ActionResult<IEnumerable<TransactionHistoryDto>> transactionHistory = controller.GetTrasaction(_context.Customers.First().Id);

            List<String> customerAccounts = _context.Customers.First().Accounts.Select(account => account.AccountNumber).ToList();

            OkObjectResult objectResult = Assert.IsType<OkObjectResult>(transactionHistory.Result);
            List<TransactionHistoryDto> actual = ((IEnumerable<TransactionHistoryDto>)objectResult.Value).ToList();

            List<TransactionHistoryDto> acceptedDto = _transactionHistoryDtos.Where(transactionHistory => customerAccounts.Contains(transactionHistory.SourceAccountNumber) || customerAccounts.Contains(transactionHistory.DestinationAccountNumber)).ToList();
            var model = Assert.IsAssignableFrom<IEnumerable<TransactionHistoryDto>>(actual);
            
            Assert.Equal(acceptedDto.Count(), model.Count());

            foreach (TransactionHistoryDto transactionHistoryDto in actual) 
            {
                Assert.Equal(true, acceptedDto.Contains(transactionHistoryDto));
            }

            //Assert.Equal(resultDto, model);
        }

        [Fact]
        public void NoTransactionTest()
        {
            var controller = new CustomerController(_employeeServiceImp);
            ActionResult<IEnumerable<TransactionHistoryDto>> transactionHistory = controller.GetTrasaction(_context.Customers.Last().Id);

            List<String> customerAccounts = _context.Customers.Last().Accounts.Select(account => account.AccountNumber).ToList();

            OkObjectResult objectResult = Assert.IsType<OkObjectResult>(transactionHistory.Result);
            List<TransactionHistoryDto> actual = ((IEnumerable<TransactionHistoryDto>)objectResult.Value).ToList();

            List<TransactionHistoryDto> acceptedDto = _transactionHistoryDtos.Where(transactionHistory => customerAccounts.Contains(transactionHistory.SourceAccountNumber) || customerAccounts.Contains(transactionHistory.DestinationAccountNumber)).ToList();
            var model = Assert.IsAssignableFrom<IEnumerable<TransactionHistoryDto>>(actual);

            Assert.Equal(acceptedDto.Count(), model.Count());

            foreach (TransactionHistoryDto transactionHistoryDto in actual)
            {
                Assert.Equal(true, acceptedDto.Contains(transactionHistoryDto));
            }

        }

        [Fact]
        public void BetetTest()
        {
            int preBalance = _context.Accounts.First(account => account.Id == 1).Balance;

            var controller = new TransactionController(_employeeServiceImp);
            AddTakeOutMoneyDto dto = new AddTakeOutMoneyDto();
            dto.Amount = 100000;
            dto.AccountId = 1;
            dto.Type = "ADD";
            Task<IActionResult> transactionHistory = controller.Personal(dto);


            OkResult objectResult = Assert.IsType<OkResult>(transactionHistory.Result);
            Assert.Equal(preBalance + 100000, _context.Accounts.First(account => account.Id == 1).Balance);

        }

        [Fact]
        public void KivetTeszt()
        {
            int preBalance = _context.Accounts.First(account => account.Id == 1).Balance;

            var controller = new TransactionController(_employeeServiceImp);
            AddTakeOutMoneyDto dto = new AddTakeOutMoneyDto();
            dto.Amount = 100000;
            dto.AccountId = 1;
            dto.Type = "TAKEOUT";
            Task<IActionResult> transactionHistory = controller.Personal(dto);


            OkResult objectResult = Assert.IsType<OkResult>(transactionHistory.Result);
            Assert.Equal(preBalance - 100000, _context.Accounts.First(account => account.Id == 1).Balance);

        }

        [Fact]
        public void KivetToMuchTeszt()
        {
            int preBalance = _context.Accounts.First(account => account.Id == 1).Balance;

            var controller = new TransactionController(_employeeServiceImp);
            AddTakeOutMoneyDto dto = new AddTakeOutMoneyDto();
            dto.Amount = 5000000;
            dto.AccountId = 1;
            dto.Type = "TAKEOUT";
            Task<IActionResult> transactionHistory = controller.Personal(dto);


            OkResult objectResult = Assert.IsType<OkResult>(transactionHistory.Result);
            Assert.Equal(preBalance, _context.Accounts.First(account => account.Id == 1).Balance);

        }

        [Fact]
        public void Transactiontest()
        {
            int preBalanceSource = _context.Accounts.First(account => account.AccountNumber == "1111-1111-1111-1111").Balance;
            int preBalanceDestination = _context.Accounts.First(account => account.AccountNumber == "3333-3333-3333-3333").Balance;

            var controller = new TransactionController(_employeeServiceImp);
            CreateTransactionDto dto = new CreateTransactionDto();
            dto.BenificaryName = "Test Elek";
            dto.SourceAccountNumber = "1111-1111-1111-1111";
            dto.DestinationAccountNumber = "3333-3333-3333-3333";
            dto.Message = "Proba trans";
            dto.TransactionTotal = 100000;

            Task<IActionResult> createTransaction = controller.CeateTransaction(dto);

            OkResult objectResult = Assert.IsType<OkResult>(createTransaction.Result);
            Assert.Equal(preBalanceSource - 100000, _context.Accounts.First(account => account.AccountNumber == "1111-1111-1111-1111").Balance);
            Assert.Equal(preBalanceDestination + 100000, _context.Accounts.First(account => account.AccountNumber == "3333-3333-3333-3333").Balance);
            Assert.Equal(_transactionHistoryDtos.Count() + 1, _context.Transactions.Count());

        }
    }
}