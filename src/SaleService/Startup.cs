using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N8T.Infrastructure;
using N8T.Infrastructure.EfCore;
using N8T.Infrastructure.Validator;
using SaleService.Core.Infrastructure.Persistence;

namespace SaleService
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
            services.AddHttpContextAccessor()
                .AddCustomMediatR<Startup>()
                .AddCustomValidators<Program>()
                .AddCustomDbContext<MainDbContext, Startup>(Configuration.GetConnectionString("sqlserver"))
                .AddTransient<IDbConnection>(_ => new SqlConnection(Configuration.GetConnectionString("sqlserver")))
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
