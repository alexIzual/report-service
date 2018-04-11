using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace ReportService.Test
{
    public class ReportBuilderTests
    {
        private const string WL = "--------------------------------------------";

        [Fact]
        public async void MakeReportAsync_Test()
        {
            ReportBuilder reportBuilder = new ReportBuilder(2018, 1);

            var employees = GetTestEmployees();

            var result = await reportBuilder.MakeReportAsync(employees);

            var report = UTF8Encoding.UTF8.GetString(result);

            Assert.Equal(GetExpectedReport(employees), report);
        }

        [Fact]
        public void GetFormattedSalary_Test()
        {
            decimal salary = 50000.75m;

            ReportBuilder reportBuilder = new ReportBuilder(2018, 1);

            string result = reportBuilder.GetFormattedSalary(salary);

            Assert.Equal(GetFormattedSalary(salary), result);
        }

        [Fact]
        public void GetFormattedDate_Test()
        {
            int year = 2018;
            int month = 1;

            ReportBuilder reportBuilder = new ReportBuilder(year, month);

            string result = reportBuilder.GetFormattedDate();

            Assert.Equal(GetFormattedDate(year, month), result);
        }


        private IEnumerable<Employee> GetTestEmployees()
        {
            var emps = new List<Employee>();

            for (int i = 0; i < 10; i++)
            {
                emps.Add(new Employee()
                {
                    Name = "Андрей Сергеевич Бубнов",
                    Department = i % 2 == 0 ? "ИТ" : "ФинОтдел",
                    Inn = Guid.NewGuid().ToString(),
                    Salary = 10000
                });
            }

            return emps;
        }

        private string GetExpectedReport(IEnumerable<Employee> employees)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(GetFormattedDate(2018, 1));

            var groupedByDepartment = employees.GroupBy(q => q.Department);
            foreach (var department in groupedByDepartment)
            {
                AppendDepartmentHeader(result, department);

                foreach (var employee in department)
                    AppendEmployeeSalary(result, employee);

                AppendTotalSalaryByDepartment(result, department);
            }
            AppendTotalSalaryByEnterprise(result, groupedByDepartment);

            return result.ToString();
        }

        private void AppendEmployeeSalary(StringBuilder sb, Employee employee)
        {
            sb.AppendLine();
            sb.AppendLine(employee.Name + " " + GetFormattedSalary(employee.Salary.Value));
        }
        
        private void AppendTotalSalaryByDepartment(StringBuilder sb, IGrouping<string, Employee> department)
        {
            sb.AppendLine();
            sb.AppendLine("Всего по отделу " + GetFormattedSalary(department.Sum(s => s.Salary.Value)));
        }
        
        private void AppendTotalSalaryByEnterprise(StringBuilder sb, IEnumerable<IGrouping<string, Employee>> groupedByDepartment)
        {
            sb.AppendLine();
            sb.AppendLine(WL);
            sb.AppendLine();
            sb.AppendLine("Всего по предприятию " + GetFormattedSalary(groupedByDepartment.Sum(s => s.Sum(e => e.Salary.Value))));
        }
        
        private void AppendDepartmentHeader(StringBuilder sb, IGrouping<string, Employee> department)
        {
            sb.AppendLine();
            sb.AppendLine(WL);
            sb.AppendLine();
            sb.AppendLine(department.Key);
        }

        public string GetFormattedSalary(decimal salary)
        {
            return Math.Round(salary, 0, MidpointRounding.AwayFromZero) + "р";
        }

        public string GetFormattedDate(int year, int month)
        {
            var date = new DateTime(year, month, 1);

            return date.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        }
    }
}
