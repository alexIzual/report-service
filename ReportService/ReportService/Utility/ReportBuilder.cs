using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    public class ReportBuilder
    {
        private string _date;

        private StringBuilder SBuilder { get; set; }

        public ReportBuilder(int year, int month)
        {
            var date = new DateTime(year, month, 1);

            _date = date.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
        }

        public byte[] MakeReport(IEnumerable<Employee> employees)
        {
            return MakeReportAsync(employees).Result;
        }

        public Task<byte[]> MakeReportAsync(IEnumerable<Employee> employees)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var employee in employees)
                {
                    employee.BuhCode = EmpCodeResolver.GetCode(employee.Inn).Result;
                    employee.Salary = employee.Salary();

                    //actions.Add((new ReportFormatter(null).NL, new Employee()));
                    //actions.Add((new ReportFormatter(null).WL, new Employee()));
                    //actions.Add((new ReportFormatter(null).NL, new Employee()));
                    //actions.Add((new ReportFormatter(null).WD, new Employee() { Department = depName }));
                    //for (int i = 1; i < emplist.Count(); i++)
                    //{
                    //    actions.Add((new ReportFormatter(emplist[i]).NL, emplist[i]));
                    //    actions.Add((new ReportFormatter(emplist[i]).WE, emplist[i]));
                    //    actions.Add((new ReportFormatter(emplist[i]).WT, emplist[i]));
                    //    actions.Add((new ReportFormatter(emplist[i]).WS, emplist[i]));
                    //}
                    //actions.Add((new ReportFormatter(null).NL, null));
                    //actions.Add((new ReportFormatter(null).WL, null));
                }

                return UTF8Encoding.UTF8.GetBytes(SBuilder.ToString());
            });
        }
    }
}
