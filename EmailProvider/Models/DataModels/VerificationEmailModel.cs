using Newtonsoft.Json;

namespace EmailProvider.Models.DataModels
{
    public class VerificationEmailModel : IdentityEmailModel
    {
        public string EmailType = "Verification";
    }
}
