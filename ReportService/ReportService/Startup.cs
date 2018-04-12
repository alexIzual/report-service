using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReportService.Clients;
using ReportService.DAL;
using ReportService.Services;
using NLog.Web;
using NLog.Extensions.Logging;

namespace ReportService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();

            services.AddScoped<IEmployeeRepository>(s =>
                new EmployeeRepository(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IBuhApiClient, BuhApiClient>();
            services.AddScoped<ISalaryApiClient, SalaryApiClient>();
            services.AddScoped<IReportHelperService, ReportHelperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            env.ConfigureNLog("nlog.config");

            loggerFactory.AddNLog();

            app.AddNLogWeb();
        }
    }
}
