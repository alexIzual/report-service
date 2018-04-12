using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public interface IReportHelperService
    {
        /// <summary>
        /// Формирует бухгалтерский отчет по сотрудникам предприятия.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<byte[]> MakeReportAsync(int year, int month);
    }
}
