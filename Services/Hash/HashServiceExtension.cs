namespace ASP_Starbucks.Services.Hash
{
    public static class HashServiceExtension
    {
        public static void AddHash(this IServiceCollection services)
        {
            services.AddSingleton<IHashService, Sha1HashService>();
        }
    }
}
