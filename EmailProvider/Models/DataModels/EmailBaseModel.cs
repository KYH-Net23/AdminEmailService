namespace EmailProvider.Models.DataModels;

public abstract class EmailBaseModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Header { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;

}
