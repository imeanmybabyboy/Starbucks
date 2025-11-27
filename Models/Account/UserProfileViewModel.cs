namespace ASP_Starbucks.Models.Account
{
    public class UserProfileViewModel
    {
        public UserProfileFormModel? FormModel { get; set; }
        public Dictionary<string, string>? ModelErrors { get; set; }
    }
}
