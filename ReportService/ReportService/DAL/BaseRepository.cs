using Npgsql;
using System;
using System.Data;

namespace ReportService.DAL
{
    public abstract class BaseRepository : IBaseRepository, IDisposable
    {
        public IDbConnection Db { get; set; }

        public BaseRepository(string connectionString)
        {
            Db = new NpgsqlConnection(connectionString);
        }

        public void Dispose()
        {
            Db.Close();
        }
    }
}
