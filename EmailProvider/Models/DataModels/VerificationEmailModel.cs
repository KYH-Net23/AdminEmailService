using Newtonsoft.Json;

namespace EmailProvider.Models.DataModels
{
    public class VerificationEmailModel : IdentityEmailModel
    {
        [JsonIgnore]
        public string EmailType { get; set; } = "Verification";
    }
}
