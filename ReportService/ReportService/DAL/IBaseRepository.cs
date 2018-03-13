using System.Data;

namespace ReportService.DAL
{
    public interface IBaseRepository
    {
        IDbConnection Db { get; set; }
    }
}
