namespace ASP_Starbucks.Services.Random
{
    public static class RandomServiceExtension
    {
        public static void AddRandom(this IServiceCollection services)
        {
            services.AddSingleton<IRandomService, RandomService>();
        }
    }
}
