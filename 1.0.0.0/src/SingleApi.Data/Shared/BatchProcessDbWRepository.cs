using Dapper;
using SingleApi.Data.Base;
using SingleApi.Data.Contracts;
using SingleApi.Data.Contracts.Models.BatchProcess;
using SingleApi.Data.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Shared
{
    public class BatchProcessDbWRepository : BaseRepository, IBatchProcessDbWRepository
    {
        public const string uspSyAppSvcConfigSettingGet = "[Config].[uspSyAppSvcConfigSettingGet]";
        public const string uspSyAppSvcConfigSettingsGet = "[Config].[uspSyAppSvcConfigSettingsGet]";
        public const string uspSyAppSvcConfigSettingsByParrentGet = "[Config].[uspSyAppSvcConfigSettingsByParrentGet]";
        public const string uspSyExecProcessGIdGet = "[Config].[uspSyExecProcessGIdGet]";
        public const string uspLastProcessInstanceGet = "[Config].[uspLastProcessInstanceGet]";
        public const string uspCronFiredTriggersByJobName = "[dbo].[uspCronFiredTriggersByJobName]";

        public const string Id = "Id";
        public const string SyAppSvcCode = "SyAppSvcCode";
        public const string SettingKey = "SettingKey";
        public const string IsActive = "IsActive";
        public const string ParentSettingKey = "ParentSettingKey";
        public const string SyExecProcessStatus = "SyExecProcessStatus";
        public const string JobName = "JobName";
        public BatchProcessDbWRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IDictionary<string, string>> GetSyAppSvcConfigSettingsAsync(string syAppSvcCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SyAppSvcCode", syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add("IsActive", true, DbType.Boolean, ParameterDirection.Input);

            var result = await GetFilteredListAsync<KeyValuePair<string, string>>(
                "[Config].[uspSyAppSvcConfigSettingsGet]",
                parameters,
                CommandType.StoredProcedure)
                .ConfigureAwait(false);

            return result.ToDictionary(x => x.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<string> GetSyAppSvcConfigSettingAsync(
            string syAppSvcCode,
            string settingKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(SettingKey, settingKey, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);

            var result = await GetFirstOrDefaultAsync<string>(
                    uspSyAppSvcConfigSettingGet,
                    parameters,
                    CommandType.StoredProcedure)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IDictionary<string, string>> GetSyAppSvcConfigSettings(string syAppSvcCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);

            var result = await  GetFilteredListAsync<KeyValuePair<string, string>>(
                uspSyAppSvcConfigSettingsGet,
                parameters,
                CommandType.StoredProcedure);

            return result.ToDictionary(x => x.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetSyAppSvcConfigSettingsAsync(string syAppSvcCode, string parentSettingKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(ParentSettingKey, parentSettingKey, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);

            var result = await GetFilteredListAsync<KeyValuePair<string, string>>(
                uspSyAppSvcConfigSettingsByParrentGet,
                parameters,
                CommandType.StoredProcedure)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetSyAppSvcConfigSettings(string syAppSvcCode, string parentSettingKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(ParentSettingKey, parentSettingKey, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);

            var result = await GetFilteredListAsync<KeyValuePair<string, string>>(
                uspSyAppSvcConfigSettingsByParrentGet,
                parameters,
                CommandType.StoredProcedure);

            return result;
        }

        public async Task<Guid> GetSyExecProcessGId(string syAppSvcCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);

            var result = await GetFirstOrDefaultAsync<Guid>(
                uspSyExecProcessGIdGet,
                parameters,
                CommandType.StoredProcedure);

            return result;
        }

        public async Task<SyExecProcessInstance> GetLastProcessInstanceAsync(string syAppSvcCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);

            var result = await GetFirstOrDefaultAsync<SyExecProcessInstance>(
                uspLastProcessInstanceGet,
                parameters,
                CommandType.StoredProcedure)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<SyExecProcessInstance> GetLastProcessInstanceAsync(
            string syAppSvcCode,
            SyExecProcessStatus status)
        {
            var parameters = new DynamicParameters();
            parameters.Add(SyAppSvcCode, syAppSvcCode, DbType.String, ParameterDirection.Input);
            parameters.Add(IsActive, true, DbType.Boolean, ParameterDirection.Input);
            parameters.Add(SyExecProcessStatus, (short)status, DbType.Int16, ParameterDirection.Input);

            var result = await GetFirstOrDefaultAsync<SyExecProcessInstance>(
                uspLastProcessInstanceGet,
                parameters,
                CommandType.StoredProcedure)
                .ConfigureAwait(false);

            return result;
        }

        public async Task Insert(SyExecProcessInstance model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SyExecProcessGId", model.SyExecProcessGId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("InstanceName", model.InstanceName, DbType.String, ParameterDirection.Input);
            parameters.Add("ExecMachineHostIpAddr", model.ExecMachineHostIpAddr, DbType.String, ParameterDirection.Input);
            parameters.Add("ExecMachineHostName", model.ExecMachineHostName, DbType.String, ParameterDirection.Input);
            parameters.Add("ExecProcessStarted", model.ExecProcessStarted, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("ExecProcessEnded", model.ExecProcessEnded, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("SyExecProcessStatusId", (short)model.SyExecProcessStatusId, DbType.Int16, ParameterDirection.Input);
            parameters.Add("IsDeleted", model.IsDeleted, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("CreatedBy", model.CreatedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ModifiedBy", model.ModifiedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("DateCreated", model.DateCreated, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("DateModified", model.DateModified, DbType.DateTime2, ParameterDirection.Input);

            var sql = @"
                        INSERT INTO [Process].[SyExecProcessInstance]
                                   ([SyExecProcessGId]
                                   ,[InstanceName]
                                   ,[ExecMachineHostIpAddr]
                                   ,[ExecMachineHostName]
                                   ,[ExecProcessStarted]
                                   ,[ExecProcessEnded]
                                   ,[SyExecProcessStatusId]
                                   ,[IsDeleted]
                                   ,[CreatedBy]
                                   ,[ModifiedBy]
                                   ,[DateCreated]
                                   ,[DateModified])
                             VALUES
                                   (@SyExecProcessGId
                                   ,@InstanceName
                                   ,@ExecMachineHostIpAddr
                                   ,@ExecMachineHostName
                                   ,@ExecProcessStarted
                                   ,@ExecProcessEnded
                                   ,@SyExecProcessStatusId
                                   ,@IsDeleted
                                   ,@CreatedBy
                                   ,@ModifiedBy
                                   ,@DateCreated
                                   ,@DateModified);

                                   SELECT Id, SyExecProcessInstanceGId FROM [Process].[SyExecProcessInstance] WHERE Id = SCOPE_IDENTITY();   
                        ";

            var res = await GetFirstOrDefaultAsync<(long Id, Guid SyExecProcessInstanceGId)>(sql, parameters, CommandType.Text);

            model.Id = res.Id;
            model.SyExecProcessInstanceGId = res.SyExecProcessInstanceGId;
        }

        public async Task Update(SyExecProcessInstance model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SyExecProcessGId", model.SyExecProcessGId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("SyExecProcessInstanceGId", model.SyExecProcessInstanceGId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("InstanceName", model.InstanceName, DbType.String, ParameterDirection.Input);
            parameters.Add("ExecMachineHostIpAddr", model.ExecMachineHostIpAddr, DbType.String, ParameterDirection.Input);
            parameters.Add("ExecMachineHostName", model.ExecMachineHostName, DbType.String, ParameterDirection.Input);
            parameters.Add("ExecProcessStarted", model.ExecProcessStarted, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("ExecProcessEnded", model.ExecProcessEnded, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("SyExecProcessStatusId", (short)model.SyExecProcessStatusId, DbType.Int16, ParameterDirection.Input);
            parameters.Add("IsDeleted", model.IsDeleted, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("CreatedBy", model.CreatedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ModifiedBy", model.ModifiedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("DateCreated", model.DateCreated, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("DateModified", model.DateModified, DbType.DateTime2, ParameterDirection.Input);

            var sql = @"
                UPDATE [Process].[SyExecProcessInstance]
                   SET [SyExecProcessGId] = @SyExecProcessGId
                      ,[InstanceName] = @InstanceName
                      ,[ExecMachineHostIpAddr] = @ExecMachineHostIpAddr
                      ,[ExecMachineHostName] = @ExecMachineHostName
                      ,[ExecProcessStarted] = @ExecProcessStarted
                      ,[ExecProcessEnded] = @ExecProcessEnded
                      ,[SyExecProcessStatusId] = @SyExecProcessStatusId
                      ,[IsDeleted] = @IsDeleted
                      ,[CreatedBy] = @CreatedBy
                      ,[ModifiedBy] = @ModifiedBy
                      ,[DateCreated] = @DateCreated
                      ,[DateModified] = @DateModified
                 WHERE [SyExecProcessInstanceGId] = @SyExecProcessInstanceGId;
                ";

            await ExecuteAsync(sql, parameters, CommandType.Text);
        }

        public async Task<IEnumerable<CronFiredTrigger>> GetCronFiredTriggersAsync(string jobName)
        {
            var parameters = new DynamicParameters();
            parameters.Add(JobName, jobName, DbType.String, ParameterDirection.Input);

            var result = await GetFilteredListAsync<CronFiredTrigger>(
                uspCronFiredTriggersByJobName,
                parameters,
                CommandType.StoredProcedure)
                .ConfigureAwait(false);

            return result;
        }
    }
}
