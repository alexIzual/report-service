using Npgsql;
using System;
using System.Data;

namespace ReportService.DAL
{
    public abstract class BaseRepository : IBaseRepository
    {
        private readonly string _connectionString;
        
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
