using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    public class ReportBuilder
    {
        private string FormattedReportDate;

        private const string WL = "--------------------------------------------";

        private StringBuilder SBuilder { get; set; }

        public ReportBuilder(int year, int month)
        {
            SBuilder = new StringBuilder();

            var date = new DateTime(year, month, 1);

            FormattedReportDate = date.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Формирует бухгалтерский отчет по сотрудникам предприятия.
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Task<byte[]> MakeReportAsync(IEnumerable<Employee> employees)
        {
            return Task.Factory.StartNew(() =>
            {
                SBuilder.AppendLine(FormattedReportDate);

                var groupedByDepartment = employees.GroupBy(q => q.Department);
                foreach (var department in groupedByDepartment)
                {
                    AppendDepartmentHeader(department);

                    foreach (var employee in department)
                        AppendEmployeeSalary(employee);

                    AppendTotalSalaryByDepartment(department);
                }
                AppendTotalSalaryByEnterprise(groupedByDepartment);

                return UTF8Encoding.UTF8.GetBytes(SBuilder.ToString());
            });
        }

        /// <summary>
        /// Добавляет к отчету информацию по сотруднику.
        /// </summary>
        /// <param name="employee"></param>
        private void AppendEmployeeSalary(Employee employee)
        {
            SBuilder.AppendLine();
            SBuilder.AppendLine(employee.Name + " " + FormattingSalary(employee.Salary.Value));
        }

        /// <summary>
        /// Добавляет к отчету сумму зарплат сотрудников отдела.
        /// </summary>
        /// <param name="department"></param>
        private void AppendTotalSalaryByDepartment(IGrouping<string, Employee> department)
        {
            SBuilder.AppendLine();
            SBuilder.AppendLine("Всего по отделу " + FormattingSalary(department.Sum(s => s.Salary.Value)));
        }

        /// <summary>
        /// Добавляет к отчету сумму зарплат сотрудников предприятия.
        /// </summary>
        /// <param name="groupedByDepartment"></param>
        private void AppendTotalSalaryByEnterprise(IEnumerable<IGrouping<string, Employee>> groupedByDepartment)
        {
            SBuilder.AppendLine();
            SBuilder.AppendLine(WL);
            SBuilder.AppendLine();
            SBuilder.AppendLine("Всего по предприятию " + FormattingSalary(groupedByDepartment.Sum(s => s.Sum(e => e.Salary.Value))));
        }

        /// <summary>
        /// Добавляет к отчету название отдела.
        /// </summary>
        /// <param name="department"></param>
        private void AppendDepartmentHeader(IGrouping<string, Employee> department)
        {
            SBuilder.AppendLine();
            SBuilder.AppendLine(WL);
            SBuilder.AppendLine();
            SBuilder.AppendLine(department.Key);
        }

        /// <summary>
        /// Форматирует зарплату.
        /// </summary>
        /// <param name="salary"></param>
        /// <returns></returns>
        private string FormattingSalary(decimal salary)
        {
            return Math.Round(salary, 0, MidpointRounding.AwayFromZero) + "р";
        }
    }
}
