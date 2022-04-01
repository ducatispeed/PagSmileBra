using System.Data;

namespace SingleApi.Data.Contracts.Base
{
    public interface IBaseRepository : IDisposable
    {
        string DbConnectionString { get; set; }
        IDbConnection GetDbConnection(string dbConnectionString = "");
    }
}
