namespace EmailProvider.Models.DataModels
{
    public class IdentityEmailModel : EmailBaseModel
    {
        public Uri? Uri { get; set; }
        public object? ExtraContent { get; set; }

    }
}
