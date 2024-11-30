using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlateauMed.Core;

namespace PlateauMed.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionstring = configuration.GetConnectionString("DataDbContext");


            services.AddDbContextPool<DataDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(connectionstring, optionsBuilder => optionsBuilder.EnableRetryOnFailure());
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            services.AddEntityFrameworkSqlServer();
            services.AddTransient<DbContext, DataDbContext>();
        }
    }
}
