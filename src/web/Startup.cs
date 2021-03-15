using CurrencyExchangeAPI.Extensions;
using CurrencyExchangeAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CurrencyExchangeAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CurrencyExchangeAPI", Version = "v1" });
            });

            //Use Mediator pattern
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //ToDo: Refactor - If the number of services grow
            // Define a ServiceRegisterationExtensions to move service.add service type there
            // Use this approach to get value from appsettings or Options pattern (IOptionsMonitor)
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0
            var logFormatterOptions = Configuration.GetSection("LogFormatterOptions").Get<LogFormatterOptions>();
            services.AddSingleton(logFormatterOptions);

            services.AddSingleton<ILogFormatter, LogFormatter>();
            services.AddSingleton<IExchangeRateService, ExchangeRateService>();

            services.AddHttpClient<IHttpClientFactoryExtensions, HttpClientFactoryExtensions>();


            services.Configure<ConfigurationOptions>(Configuration.GetSection(ConfigurationOptions.ThirdPartyApi));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyExchangeAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
