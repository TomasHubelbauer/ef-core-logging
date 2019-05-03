using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    }
  }

  public class Item
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
