using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReportService.Controllers;
using ReportService.Domain;
using ReportService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReportService.Test
{
    public class ReportControllerTests
    {
        [Fact(DisplayName = "ReportController.Download_WithValidDate_ReturnFileTest")]
        public async void Download_WithValidDate_ReturnFileTest()
        {
            int validYear = 2018;
            int validMonth = 2;

            var mockLogg = Mock.Of<ILogger<ReportController>>();

            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(s => s.GetEmployeesWithSalaryAsync(validYear, validMonth))
                .Returns(Task.FromResult(GetTestEmployeesWithSalary()));

            var mockReportService = new Mock<ReportHelperService>(mockEmployeeService.Object).Object;

            var controller = new ReportController(mockReportService, mockLogg);

            var result = await controller.Download(validYear, validMonth);

            Assert.IsType<FileContentResult>(result);
            Assert.True(((FileContentResult)result).FileContents.Length > 0);
        }

        [Fact(DisplayName = "ReportController.Download_WithNotValidDate_ReturnBadRequestTest")]
        public async void Download_WithNotValidDate_ReturnBadRequestTest()
        {
            int notValidYear = 10000;
            int notValidMonth = 15;

            var mockLogg = Mock.Of<ILogger<ReportController>>();

            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(s => s.GetEmployeesWithSalaryAsync(notValidYear, notValidMonth))
                .Returns(Task.FromResult(GetTestEmployeesWithSalary()));

            var mockReportService = new Mock<ReportHelperService>(mockEmployeeService.Object).Object;

            var controller = new ReportController(mockReportService, mockLogg);

            var result = await controller.Download(notValidYear, notValidMonth);

            Assert.IsType<BadRequestResult>(result);
        }

        private IEnumerable<Employee> GetTestEmployeesWithSalary()
        {
            var emps = new List<Employee>();

            for (int i = 0; i < 1500; i++)
            {
                emps.Add(new Employee()
                {
                    Name = "������ ��������� ������",
                    Department = "��������",
                    Inn = Guid.NewGuid().ToString(),
                    Salary = GetTestSalary()
                });
            }

            return emps;
        }

        private decimal GetTestSalary()
        {
            var rand = new Random();

            return rand.Next(10000, 150000);
        }
    }
}
