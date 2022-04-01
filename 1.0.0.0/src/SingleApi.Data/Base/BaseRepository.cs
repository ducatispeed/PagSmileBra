using Dapper;
using SingleApi.Data.Contracts.Base;
using System.Data;
using System.Data.SqlClient;
using CustomLoggerLib.Crosscutting;

namespace SingleApi.Data.Base
{
    public class BaseRepository : IBaseRepository
    {
        #region Ctor

        public BaseRepository(string connectionString)
        {
            DbConnectionString = connectionString.NotNull(nameof(connectionString));
        }

        protected BaseRepository(IDbConnection connection)
        {
            DbConnectionString = connection?.ConnectionString.NotNull(nameof(connection.ConnectionString));
        }

        #endregion

        #region Properties

        public string DbConnectionString { get; set; }

        #endregion

        #region Public Methods

        public IDbConnection GetDbConnection(string dbConnectionStr = "")
        {
            return new SqlConnection(DbConnectionString);
        }

        public async Task<Type> ExecuteScalarAsync<Type>(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                var result =
                    await dbCon.ExecuteScalarAsync<Type>(
                        sql,
                        commandType: commandType,
                        param: parameters);

                return result;
            }
        }

        public async Task<Type> GetFirstOrDefaultAsync<Type>(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                var result =
                    await dbCon.QueryFirstOrDefaultAsync<Type>(
                        sql,
                        commandType: commandType,
                        param: parameters);

                return result;
            }
        }

        public async Task ExecuteAsync(
            string sql,
            object param = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                await dbCon.ExecuteAsync(sql, param, commandType: commandType);
            }
        }

        public async Task<IEnumerable<T>> GetFilteredListAsync<T>(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                var result = await dbCon.QueryAsync<T>(sql, parameters, commandType: commandType);
                return result;
            }
        }

        public async Task<bool> GetBoolAsync(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                return await dbCon.QueryFirstOrDefaultAsync<bool>(sql, parameters, commandType: commandType);
            }
        }

        public async Task<short> GetShortAsync(
    string sql,
    object parameters = null,
    CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                return await dbCon.QueryFirstOrDefaultAsync<short>(sql, parameters, commandType: commandType);
            }
        }

        public async Task<T> QuerySingleAsync<T>(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                return await dbCon.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType);
            }
        }

        public async Task<Dictionary<string, string>> GetFirstOrDefaultAsDictionaryAsync(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var dbCon = GetDbConnection())
            {
                var result = await dbCon.QueryFirstOrDefaultAsync(sql, parameters, commandType: commandType) as IDictionary<string, object>;
                return result?.ToDictionary(d => d.Key, d => d.Value?.ToString());
            }
        }

        #endregion

        public void Dispose()
        {
        }
    }
}
