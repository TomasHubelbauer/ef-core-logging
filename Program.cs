using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    public DbSet<Item> Items { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer($@"Server=(localdb)\{nameof(ef_core_logging)};Database={nameof(ef_core_logging)};");
      optionsBuilder.EnableDetailedErrors();
      optionsBuilder.EnableSensitiveDataLogging();

      var serviceCollection = new ServiceCollection();
      serviceCollection.AddLogging(builder =>
        builder
          .AddConsole()
          .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Trace)
      );

      var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
      optionsBuilder.UseLoggerFactory(loggerFactory);
    }
  }

  public class Item
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
