using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.DAL
{
    public interface IBaseRepository
    {
        IDbConnection Db { get; set; }
    }
}
