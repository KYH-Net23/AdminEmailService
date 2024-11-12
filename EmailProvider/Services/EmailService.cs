using EmailProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailProvider.Services;

public class EmailService(DataContext dbContext)
{
    public async Task<List<Email>> GetEmailsAsync()
    {
        var emails = await dbContext.Emails.ToListAsync();
        return emails;
    }
}
