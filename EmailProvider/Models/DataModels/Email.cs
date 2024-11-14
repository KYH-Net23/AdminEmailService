namespace EmailProvider.Models.DataModels;

public class Email
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
