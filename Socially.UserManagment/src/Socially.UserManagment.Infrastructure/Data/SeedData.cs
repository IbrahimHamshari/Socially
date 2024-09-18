using Microsoft.EntityFrameworkCore;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.ContributorAggregate;

namespace Socially.UserManagment.Infrastructure.Data;
public static class SeedData
{
  public static readonly User User1 = new("username1","example@example.com","password1","firstname1","lastname1");
  public static readonly User User2 = new("username2","example@example.com","password2","firstname2","lastname2");

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
