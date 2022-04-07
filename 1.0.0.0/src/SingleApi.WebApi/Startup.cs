using System.Reflection;
using System.Text.Json.Serialization;
using SfpSharedLib.Data.SqlServer;
using SingleApi.Data.Contracts.Shared;
using SingleApi.Data.Shared;
using SingleApi.Infrastructure.Constants;
using SingleApi.Svc.Paysafe.Services;
using SingleApi.Svc.Contracts.Paysafe.Services;
using System.Text.Json;
using SingleApi.Svc.Contracts.Paysafe.Commands;
using SingleApi.Svc.Contracts.Paysafe.Models.View;
using MediatR;
using SingleApi.Svc.Paysafe.Handlers;
using SingleApi.Svc.Contracts.Paysafe.Services;
using SfpSharedLib.CrossConcerns.Logging.WsLogger;
using SfpSharedLib.CrossConcerns.Logging.CustomLogger;
using SingleApi.Infrastructure.Wrappers;
using System.Data.SqlClient;
using Serilog;
using SingleApi.WebApi.Config.Extensions;

namespace SingleApi.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var assemblies = GetAssemblies().ToArray();

            //services.AddCors();
            services.AddTransient<IPaysafeApiService, PaysafeApiService>();

            services
                .AddMediatR(assemblies)
                //.AddSwagger(_configuration)
                //.AddPrincipal()
                //.AddRedisCache()
                //.AddDataDictLocalization(_configuration)
                .AddMemoryCache()
                //.AddData(assemblies)
                //.AddApplication(assemblies)
                //.AddAuth(_configuration)
                //.AddErrorHandling(_configuration)
                .AddHttpClient()
                .AddRequestLogging()
                .AddControllers()
                .AddJsonOptions(
                    options =>
                    {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });

            services.AddTransient<NetworkHelper>();
            services.AddSingleton<WSLoggerWrapper>(x => AddWsLogger(x));
            CreateLogger();
            services.AddLogging(x => x.AddSerilog(dispose: true));

            string partnerConnectionString = _configuration.GetConnectionString(ConnectionNames.PartnerConnectionName);
            string prodConnectionString = _configuration.GetConnectionString(ConnectionNames.PRODConnectionName);

            services.AddTransient<IPartnerRepository, PartnerRepository>(provider => new PartnerRepository(partnerConnectionString));
            services.AddScoped<IPaysafeApiService, PaysafeApiService>();
            services.AddTransient<IProdRepository, ProdRepository>(provider => new ProdRepository(prodConnectionString));
            services.AddMediatR(Assembly.GetExecutingAssembly());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app
                //.UseSwaggerEndpoints()
                //.UseHttpsRedirection()
                .UseRouting()
                //.UseDataDictLocalization()
                //.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
                //.UseAuth()
                .UseRequestLogging()
                //.UseErrorHandling()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(Startup).Assembly;
            yield return typeof(CallBackPaymentHandler).Assembly;
            yield return typeof(CallBackPaymentHandleCommand).Assembly;
            yield return typeof(GetPaymentStatusHandleCommand).Assembly;
            yield return typeof(GetPaymentStatusHandleViewModel).Assembly;
        }

        WSLoggerWrapper AddWsLogger(IServiceProvider services)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();

            var wsLogger = new WSLoggerWrapper((NetworkHelper)services.GetService(typeof(NetworkHelper)), loggerFactory.CreateLogger<WSLoggerWrapper>());
            try
            {
                var syAppSvcCode = _configuration.GetValue<string>($"{AppSettings.Name}:{AppSettings.SyAppSvcCode}");
                wsLogger.InitWsLogger(_configuration.GetConnectionString(ConnectionNames.BatchProcessDbWConnectionName), _configuration.GetConnectionString(ConnectionNames.WSLogConnectionName), syAppSvcCode);
            }
            catch
            {
                //Any log operation SHOULD NOT stop the whole flow, failures should go to event log and/or be notified.
            }
            return wsLogger;
        }

        void CreateLogger()
        {
            try
            {
                var syAppSvcCode = _configuration.GetValue<string>($"{AppSettings.Name}:{AppSettings.SyAppSvcCode}");

                using (var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionNames.BatchProcessDbWConnectionName)))
                {
                    Log.Logger = new LoggerConfiguration().ReadFrom.DataBaseSettings(connection, syAppSvcCode).CreateLogger();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
