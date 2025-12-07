using ASP_Starbucks.Services.Random;

namespace ASP_Starbucks.Services.Salt
{
    public class AbcSaltService(IRandomService randomService) : ISaltService
    {
        public string GetSalt(int? length = null)
        {
            length ??= 16;
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            char[] chars = new char[length.Value];

            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)(97 + randomService.RandomInt() % 26);
            }

            return new string(chars);
        }
    }
}
