using Dfe.PersonsApi.Application.Common.Interfaces;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using Dfe.PersonsApi.Infrastructure;
using Dfe.PersonsApi.Infrastructure.QueryServices;
using Dfe.PersonsApi.Infrastructure.Repositories;
using Dfe.PersonsApi.Infrastructure.Security.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddPersonsApiInfrastructureDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            //Repos
            services.AddScoped<ITrustRepository, TrustRepository>();
            services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            services.AddScoped<IConstituencyRepository, ConstituencyRepository>();

            // Query Services
            services.AddScoped<IEstablishmentQueryService, EstablishmentQueryService>();
            services.AddScoped<ITrustQueryService, TrustQueryService>();

            //Cache service
            services.AddServiceCaching(config);

            //Db
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<MstrContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<MopContext>(options =>
                options.UseSqlServer(connectionString));
            // Health checks
            services.AddHealthChecks()
                .AddDbContextCheck<MstrContext>("PersonsApi - Academies Database");

            // Authentication
            services.AddCustomAuthorization(config);

            return services;
        }
    }
}