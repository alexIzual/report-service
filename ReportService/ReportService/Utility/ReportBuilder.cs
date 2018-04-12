using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    public class ReportBuilder
    {
        private int _year;
        private int _month;

        private const string WL = "--------------------------------------------";

        private StringBuilder SBuilder { get; set; }

        public ReportBuilder(int year, int month)
        {
            SBuilder = new StringBuilder();

            _year = year;
            _month = month;
        }

        /// <summary>
        /// Формирует бухгалтерский отчет по сотрудникам предприятия.
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Task<byte[]> BuildReportAsync(IEnumerable<Employee> employees)
        {
            return Task.Factory.StartNew(() =>
            {
                SBuilder.AppendLine(GetFormattedDate());

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
            SBuilder.AppendLine(employee.Name + " " + GetFormattedSalary(employee.Salary.Value));
        }

        /// <summary>
        /// Добавляет к отчету сумму зарплат сотрудников отдела.
        /// </summary>
        /// <param name="department"></param>
        private void AppendTotalSalaryByDepartment(IGrouping<string, Employee> department)
        {
            SBuilder.AppendLine();
            SBuilder.AppendLine("Всего по отделу " + GetFormattedSalary(department.Sum(s => s.Salary.Value)));
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
            SBuilder.AppendLine("Всего по предприятию " + GetFormattedSalary(groupedByDepartment.Sum(s => s.Sum(e => e.Salary.Value))));
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
        public string GetFormattedSalary(decimal salary)
        {
            return Math.Round(salary, 0, MidpointRounding.AwayFromZero) + "р";
        }

        /// <summary>
        /// Вернет отформатированную дату (MMMM yyyy).
        /// </summary>
        /// <returns></returns>
        public string GetFormattedDate()
        {
            var date = new DateTime(_year, _month, 1);

            return date.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        }
    }
}
