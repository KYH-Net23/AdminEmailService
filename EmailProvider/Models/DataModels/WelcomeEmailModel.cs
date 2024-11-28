using Newtonsoft.Json;

namespace EmailProvider.Models.DataModels
{
    public class WelcomeEmailModel : IdentityEmailModel
    {
        public string EmailType = "Welcome";
    }
}
