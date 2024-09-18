using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.Messaging.UseCases.Contributors.List;
public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
