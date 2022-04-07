using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SingleApi.Data.Contracts.Models;
using SingleApi.Infrastructure.Enums;
using SingleApi.Infrastructure.Wrappers;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Models.Responses;
using SingleApi.Svc.Contracts.Paysafe.Services;

namespace SingleApi.Svc.Paysafe.Services
{
    public class PaysafeApiService : IPaysafeApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly WSLoggerWrapper _wSLoggerWrapper;

        public PaysafeApiService(IHttpClientFactory httpClientFactory, WSLoggerWrapper wSLoggerWrapper)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            _wSLoggerWrapper = wSLoggerWrapper;
        }
        public async Task<CallbackPaymentHandleResponse> CallbackPaymentHandleAsync(CallbackPaymentHandleRequest request, IEnumerable<GatewayPspConfiguration> config)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", config.FirstOrDefault(x => x.ConfigurationName == "Authorization")?.Value);
            var json = JsonConvert.SerializeObject(request, _jsonSettings);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var url = config.FirstOrDefault(x => x.ConfigurationName == "CallbackHandleUrl")?.Value + $"/{request.OperationId}";
            var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();
            var responseContent = await response.Content.ReadAsStringAsync();
            _wSLoggerWrapper.LogRequest(
                requestUrl: url,
                requestMethod: "POST",
                requestContentType: content.Headers.ContentType.ToString(),
                requestContentEncoding: content.Headers.ContentEncoding.ToString(),
                requestContentBody: await content.ReadAsStringAsync(),
                responseContent: responseContent,
                isExternalApiCall: true);

            return JsonConvert.DeserializeObject<CallbackPaymentHandleResponse>(await response.Content.ReadAsStringAsync());
        }
        public async Task<PostPaymentHandleResponse> PostPaymentHandleAsync(PostPaymentHandleRequest request, IEnumerable<GatewayPspConfiguration> config)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", config.FirstOrDefault(x => x.ConfigurationName == "Authorization")?.Value);
            
            var url = config.FirstOrDefault(x => x.ConfigurationName == "PaymentHandleUrl")?.Value;

            var json = JsonConvert.SerializeObject(request, _jsonSettings);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();

            var responseContent = await response.Content.ReadAsStringAsync();

            _wSLoggerWrapper.LogRequest(
                requestUrl: url,
                requestMethod: "POST",
                requestContentType: content.Headers.ContentType.ToString(),
                requestContentEncoding: content.Headers.ContentEncoding.ToString(),
                requestContentBody: await content.ReadAsStringAsync(),
                responseContent: responseContent,
                isExternalApiCall: true);

            if(string.IsNullOrEmpty(responseContent))
            {
                return new PostPaymentHandleResponse
                {
                    Error = new PostPaymentHandleResponse.ErrorModel
                    {
                        Code = response.StatusCode.ToString(),
                        Message = response.ReasonPhrase
                    }
                };
            }

            return JsonConvert.DeserializeObject<PostPaymentHandleResponse>(responseContent);
            
        }
        public async Task<GetPaymentStatusHandleResponse?> GetPaymentStatusHandleAsync(GetPaymentStatusHandleRequest request, IEnumerable<GatewayPspConfiguration> config)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", config.FirstOrDefault(x => x.ConfigurationName == "Authorization")?.Value);
            
            var url = config.FirstOrDefault(x => x.ConfigurationName == "PaymentHandleUrl")?.Value+"/"+request.PaymentId;
            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();

            var responseContent = await response.Content.ReadAsStringAsync();
            _wSLoggerWrapper.LogRequest(
                requestUrl: url,
                requestMethod: "GET",
                requestContentType: httpClient.DefaultRequestHeaders.ToString(),
                requestContentEncoding: "",
                requestContentBody: "",
                responseContent: responseContent,
                isExternalApiCall: true);

            return JsonConvert.DeserializeObject<GetPaymentStatusHandleResponse>(await response.Content.ReadAsStringAsync());
        }
        public async Task<PostPaymentHubHandleResponse?> PostPaymentHubHandleAsync(PostPaymentHubHandleRequest request, IEnumerable<GatewayPspConfiguration> config)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", config.FirstOrDefault(x => x.ConfigurationName == "Authorization")?.Value);
            var json = JsonConvert.SerializeObject(request, _jsonSettings);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var url = config.FirstOrDefault(x => x.ConfigurationName == "PaymentHandleUrl")?.Value;

            var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();
            var responseContent = await response.Content.ReadAsStringAsync();
            _wSLoggerWrapper.LogRequest(
                requestUrl: url,
                requestMethod: "POST",
                requestContentType: content.Headers.ContentType.ToString(),
                requestContentEncoding: content.Headers.ContentEncoding.ToString(),
                requestContentBody: await content.ReadAsStringAsync(),
                responseContent: responseContent,
                isExternalApiCall: true);

            return JsonConvert.DeserializeObject<PostPaymentHubHandleResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
