namespace EmailProvider.Interfaces
{
    public interface IEmailService<T>
    {
        Task SendEmailAsync(T model);
    }
}
