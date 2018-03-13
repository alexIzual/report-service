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

        private string WL = "--------------------------------------------";
        
        private StringBuilder SBuilder { get; set; }

        public ReportBuilder(int year, int month)
        {
            this.SBuilder = new StringBuilder();

            var date = new DateTime(year, month, 1);
            this.FormattedReportDate = date.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
        }

        public byte[] MakeReport(IEnumerable<Employee> employees)
        {
            return MakeReportAsync(employees).Result;
        }

        public Task<byte[]> MakeReportAsync(IEnumerable<Employee> employees)
        {
            return Task.Factory.StartNew(() =>
            {
                SBuilder.AppendLine(FormattedReportDate);

                var groupedByDepartment = employees.GroupBy(q => q.Department);
                foreach (var department in groupedByDepartment)
                {
                    SBuilder.AppendLine();
                    SBuilder.AppendLine(WL);
                    SBuilder.AppendLine();
                    SBuilder.AppendLine(department.Key);

                    foreach (var employee in department)
                    {
                        SBuilder.AppendLine();
                        SBuilder.AppendLine(employee.Name + " " + Math.Round(employee.Salary, 0, MidpointRounding.AwayFromZero) + "р");
                    }

                    SBuilder.AppendLine();
                    SBuilder.AppendLine("Всего по отделу " + Math.Round(department.Sum(s => s.Salary), 0, MidpointRounding.AwayFromZero) + "р");
                }
                SBuilder.AppendLine();
                SBuilder.AppendLine(WL);
                SBuilder.AppendLine();
                SBuilder.AppendLine("Всего по предприятию " + Math.Round(groupedByDepartment.Sum(s => s.Sum(e => e.Salary)), 0, MidpointRounding.AwayFromZero) + "р");

                return UTF8Encoding.UTF8.GetBytes(SBuilder.ToString());
            });
        }
    }
}
