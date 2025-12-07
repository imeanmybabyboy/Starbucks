namespace ASP_Starbucks.Services.Random
{
    public class RandomService : IRandomService
    {
        static readonly System.Random _random = new();

        public int RandomInt()
        {
            return _random.Next();
        }
    }
}
