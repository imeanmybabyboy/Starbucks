namespace ASP_Starbucks.Services.Kdf
{
    public interface IKdfService
    {
        string Dk(string password, string salt);
    }
}
