using System.Text.Json.Serialization;

namespace EmailProvider.Models.DataModels
{
    public class ResetPasswordModel : IdentityEmailModel
    {
        [JsonIgnore]
        public string EmailType { get; set; } = "Reset";
    }
}
