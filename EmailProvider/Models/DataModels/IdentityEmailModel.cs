namespace EmailProvider.Models.DataModels
{
    public class IdentityEmailModel : EmailBaseModel
    {
        public string PassCode { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public object? ExtraContent { get; set; }
    }
}
