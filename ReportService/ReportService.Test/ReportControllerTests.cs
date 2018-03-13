using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ReportService.Controllers;
using ReportService.DAL;
using ReportService.Domain;
using ReportService.Services;
using ReportService.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

namespace ReportService.Test
{
    public class ReportControllerTests
    {
        [Fact]
        public async void Download_ReturnsFile()
        {
            int year = 2018, month = 2;

            var mockBuhApiClient = new Mock<IBuhApiClient>();
            mockBuhApiClient.Setup(s => s.GetBuhCodeByInn(It.IsAny<string>()))
                .Returns(Task.FromResult(GetTestBuhCode()));

            var mockSalaryApiClient = new Mock<ISalaryApiClient>();
            mockSalaryApiClient.Setup(s => s.GetSalary(It.IsAny<Employee>()))
                .Returns(Task.FromResult(GetTestSalary()));

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(s => s.GetEmployeesAsync())
                .Returns(Task.FromResult(GetTestEmployees()));

            var cache = new MemoryCache(new MemoryCacheOptions());

            var mockService = new Mock<EmployeeService>(mockRepo.Object, cache, mockSalaryApiClient.Object, mockBuhApiClient.Object);
            
            var controller = new ReportController(mockService.Object);

            var result = await controller.Download(year, month);

            Assert.IsType<FileContentResult>(result);

            Assert.True(((FileContentResult)result).FileContents.Length > 0);
        }

        private IEnumerable<Employee> GetTestEmployees()
        {
            var emps = new List<Employee>();
            emps.Add(new Employee()
            {
                Name = "Андрей Сергеевич Бубнов",
                Department = "ФинОтдел",
                Inn = "123"
            });
            emps.Add(new Employee()
            {
                Name = "Андрей Сергеевич Бубнов",
                Department = "ФинОтдел",
                Inn = "321"
            });
            emps.Add(new Employee()
            {
                Name = "Фрол Романович Козлов",
                Department = "ИТ",
                Inn = "213"
            });
            emps.Add(new Employee()
            {
                Name = "Арвид Янович Пельше",
                Department = "ИТ",
                Inn = "312"
            });
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
