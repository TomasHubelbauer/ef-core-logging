using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ef_core_logging
{
  class Program
  {
    static async Task Main(string[] args)
    {
      using (var appDbContext = new AppDbContext())
      {
        await appDbContext.Database.EnsureDeletedAsync();
        await appDbContext.Database.EnsureCreatedAsync();
        await appDbContext.Items.AddAsync(new Item { Name = "TEST" });
        await appDbContext.SaveChangesAsync();
      }
    }
  }

  public class AppDbContext : DbContext
  {
    private static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
    public DbSet<Item> Items { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer($@"Server=(localdb)\{nameof(ef_core_logging)};Database={nameof(ef_core_logging)};");
      optionsBuilder.EnableDetailedErrors();
      optionsBuilder.EnableSensitiveDataLogging();
      optionsBuilder.UseLoggerFactory(LoggerFactory);
    }
  }

  public class Item
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
