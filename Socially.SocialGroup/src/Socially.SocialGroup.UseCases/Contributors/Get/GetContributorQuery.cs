using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.SocialGroup.UseCases.Contributors.Get;
public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
