using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.SocialGroup.UseCases.Contributors.Delete;
public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
