using Microsoft.IdentityModel.Logging;

namespace SIO.Gateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            var identityBuilder = services.AddAuthentication();
            var authority = configuration.GetValue<string>("Identity:Authority");
            var identityConfigs = new List<IdentityConfig>();
            configuration.GetSection("Identity:Configs").Bind(identityConfigs);

            foreach(var identityConfig in identityConfigs)
            {
                identityBuilder.AddIdentityServerAuthentication(identityConfig.ApiName, options =>
                {
                    options.Authority = authority;
                    options.ApiName = "api";
                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(10);

                    if (env.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                        IdentityModelEventSource.ShowPII = true;
                    }
                });
            }

            return services;
        }
    }
}
