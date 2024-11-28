using Newtonsoft.Json;

namespace EmailProvider.Models.DataModels
{
    public class WelcomeEmailModel : IdentityEmailModel
    {
        [JsonIgnore]
        public string EmailType { get; set; } = "Welcome";
    }
}
