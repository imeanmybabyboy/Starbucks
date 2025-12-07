namespace ASP_Starbucks.Services.Salt
{
    public static class SaltServiceExtension
    {
        public static void AddSalt(this IServiceCollection services)
        {
            services.AddSingleton<ISaltService, AbcSaltService>();
        }
    }
}
