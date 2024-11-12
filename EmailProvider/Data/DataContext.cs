using EmailProvider.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace EmailProvider.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Email> Emails { get; set; }
}
