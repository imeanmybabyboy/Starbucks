namespace ASP_Starbucks.Models.User
{
    public class UserSigninViewModel
    {
        public UserSigninFormModel? FormModel { get; set; }
        public Dictionary<string, string>? ModelErrors { get; set; }
    }
}
