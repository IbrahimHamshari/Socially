using Ardalis.HttpClientTestExtensions;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Web.Contributors;
using Xunit;

namespace Socially.ContentManagment.FunctionalTests.ApiEndpoints;
[Collection("Sequential")]
public class ContributorList(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public async Task ReturnsTwoContributors()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorListResponse>("/Contributors");

    Assert.Equal(2, result.Contributors.Count);
    Assert.Contains(result.Contributors, i => i.Name == SeedData.User1.Name);
    Assert.Contains(result.Contributors, i => i.Name == SeedData.User2.Name);
  }
}
