using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StarWars.EventHub.Models;
using StarWars.EventHub.Services.Middleware;
using StarWars.EventHub.Services.Services;
using StarWars.EventHub.Services.Settings;

namespace StarWars.EventHub.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var courierSettings = Configuration.GetSection("CourierSettings").Get<CourierSettings>();

            // Register Hosted Services
            services.AddMemoryCache();
            services.AddSingleton(provider => courierSettings);
            services.AddHostedService<OrderHostedService>();
            services.AddSingleton<IEventBusService<OrderRequest>, EventBusService>();

            services.AddTransient<ICourierService, CourierService>();
            services.AddTransient<IParcelPickupService, ParcelPickupService>();
            services.AddTransient<INotificationService, NotificationService>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services
                .AddMvcCore()
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StarWars EventHub API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseJsonExceptionHandler();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarWars EventHub API");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
