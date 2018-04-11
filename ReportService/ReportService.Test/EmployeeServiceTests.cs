using Microsoft.Extensions.Caching.Memory;
using Moq;
using ReportService.Clients;
using ReportService.DAL;
using ReportService.Domain;
using ReportService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ReportService.Test
{
    public class EmployeeServiceTests
    {
        [Fact(DisplayName = "EmployeeService.GetEmployeesWithSalaryAsync")]
        public async void GetEmployeesWithSalaryAsync_Test()
        {
            int year = 2018;
            int month = 2;

            var mockBuhApiClient = new Mock<IBuhApiClient>();
            mockBuhApiClient.Setup(s => s.GetBuhCodeByInnAsync(It.IsAny<string>())).ReturnsAsync(GetTestBuhCode());

            var mockSalaryApiClient = new Mock<ISalaryApiClient>();
            mockSalaryApiClient.Setup(s => s.GetSalaryAsync(It.IsAny<Employee>(), year, month)).ReturnsAsync(GetTestSalary());

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(s => s.GetEmployeesAsync()).ReturnsAsync(GetTestEmployees());

            var cache = new MemoryCache(new MemoryCacheOptions());

            var employeeService = new EmployeeService(mockRepo.Object, cache, mockSalaryApiClient.Object, mockBuhApiClient.Object);

            IEnumerable<Employee> result = await employeeService.GetEmployeesWithSalaryAsync(year, month);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.All(q => q.Salary.HasValue));
        }

        private IEnumerable<Employee> GetTestEmployees()
        {
            var emps = new List<Employee>();

            for (int i = 0; i < 10500; i++)
            {
                emps.Add(new Employee()
                {
                    Name = "Андрей Сергеевич Бубнов",
                    Department = i % 2 == 0 ? "ИТ" : "ФинОтдел",
                    Inn = Guid.NewGuid().ToString()
                });
            }

            return emps;
        }

        private decimal GetTestSalary()
        {
            var rand = new Random();

            return rand.Next(10000, 150000);
        }

        private string GetTestBuhCode()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
