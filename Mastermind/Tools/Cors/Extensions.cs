namespace Mastermind.Tools.Cors
{
    public static class Extensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = configuration.GetRequiredSection(nameof(CorsOptions)).Get<CorsOptions>();
            if (corsOptions.IsEnabled)
            {
                services.AddCors();
            }
            return services;
        }

        public static IApplicationBuilder UseCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var corsOptions = configuration.GetRequiredSection(nameof(CorsOptions)).Get<CorsOptions>();
            if (corsOptions.IsEnabled)
            {
                app.UseCors(it => it.WithOrigins(corsOptions.Origins.ToArray()).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            }

            return app;
        }
    }
}
