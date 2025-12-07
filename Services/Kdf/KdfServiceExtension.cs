namespace ASP_Starbucks.Services.Kdf
{
    public static class KdfServiceExtension
    {
        public static void AddKdf(this IServiceCollection services)
        {
            services.AddSingleton<IKdfService, PbKdf1Service>();
        }
    }
}
