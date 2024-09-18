using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.UseCases.Contributors.Delete;
public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
