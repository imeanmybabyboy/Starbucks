namespace ASP_Starbucks.Services.Salt
{
    /// <summary>
    /// Cryptography salt is a random characters line for hashing passwords before saving to DB
    /// </summary>

    public interface ISaltService
    {
        string GetSalt(int? length = null);
    }
}
