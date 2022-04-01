using CustomLoggerLib.Domain;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data.SqlClient;

namespace SingleApi.Infrastructure.Wrappers
{
    public class WSLoggerWrapper
    {
        CustomLoggerLib.WSLogger _wSLogger;
        private readonly NetworkHelper _networkHelper;
        private readonly ILogger<WSLoggerWrapper> _logger;

        public WSLoggerWrapper(NetworkHelper networkHelper, ILogger<WSLoggerWrapper> logger)
        {
            _wSLogger = null;
            _networkHelper = networkHelper;
            _logger = logger;
        }

        public void InitWsLogger(string batchProcessWsLogConnectionString, string wsLogConnectionString, string syAppSvcCode)
        {
            try
            {
                var connectionWsLog = new SqlConnection(wsLogConnectionString);
                var connectionBatchProcess = new SqlConnection(batchProcessWsLogConnectionString);
                var syAppSvcCodeLocal = string.IsNullOrWhiteSpace(syAppSvcCode) ? ConfigurationManager.AppSettings.Get("SyAppSvcCode") : syAppSvcCode;
                _wSLogger = new CustomLoggerLib.WSLogger(connectionWsLog, connectionBatchProcess, syAppSvcCodeLocal);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}{Environment.NewLine}StackTrace:{ex.StackTrace}");
                _wSLogger = null;
            }
        }

        public void LogRequest(HttpLogItems httpLogItems)
        {
            if (_wSLogger != null)
            {
                _wSLogger.LogRequest(httpLogItems);
            }
            else
            {
                _logger.LogError("Message: WSLogger is null");
            }
        }

        public void LogRequest(string requestUrl, string requestMethod, string requestContentType, string requestContentEncoding, string requestContentBody, string responseContent, bool isExternalApiCall)
        {
            try
            {
                LogRequest(new HttpLogItems
                {
                    RequestUrl = requestUrl,
                    RequestMethod = requestMethod,
                    RequestContentType = requestContentType,
                    RequestContentBody = requestContentBody,
                    RequestContentEncoding = requestContentEncoding,
                    RequestIP = isExternalApiCall ? _networkHelper?.GetLocalIPAddress() : _networkHelper?.GetClientIPAddress(),
                    ResponseContent = responseContent,
                    RequestContentLength = requestContentBody?.Length ?? 0,
                    LogDateTime = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}{Environment.NewLine}StackTrace:{ex.StackTrace}");
            }

        }

    }
}
