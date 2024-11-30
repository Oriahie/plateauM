using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PlateauMed.Infrastructure.Background;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Managers;
using PlateauMed.Infrastructure.Repositories;
using PlateauMed.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            //services
            services.AddScoped<IPublisher, Publisher>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUtilityService, UtilityService>();



            //repositories
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationLogRepository, NotificationLogRepository>();


            //managers
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IAppointmentManager, AppointmentManager>();
            services.AddScoped<INotificationManager, NotificationManager>();


            services.AddHostedService<NotifierBackgroundService>();

        }

    }
}
