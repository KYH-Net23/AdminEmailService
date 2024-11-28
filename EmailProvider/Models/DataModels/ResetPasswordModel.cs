using System.Text.Json.Serialization;

namespace EmailProvider.Models.DataModels
{
    public class ResetPasswordModel : IdentityEmailModel
    {
        public string EmailType = "Reset";
    }
}
