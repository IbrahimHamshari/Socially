using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.Messaging.UseCases.Contributors.Delete;
public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
