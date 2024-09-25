using Microsoft.EntityFrameworkCore;
using Socially.UserManagment.Core.UserAggregate;

namespace Socially.UserManagment.Infrastructure.Data;

public static class SeedData
{
  public static readonly User User1 = new("username1", "example@example.com", "password1", "firstname1", "lastname1", false);
  public static readonly User User2 = new("username2", "example@example.com", "password2", "firstname2", "lastname2", false);

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Users.AnyAsync()) return; // DB has been seeded

    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Users.AddRange([User1, User2]);
    await dbContext.SaveChangesAsync();
  }
}
