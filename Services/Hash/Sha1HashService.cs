namespace ASP_Starbucks.Services.Hash
{
    public class Sha1HashService : IHashService
    {
        public string Digest(string input)
        {
            return Convert.ToHexString(System.Security.Cryptography.SHA1.HashData(System.Text.Encoding.UTF8.GetBytes(input)));
        }
    }
}
