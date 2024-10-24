using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.UseCases.Users.RequestVerification;
public record RequestVerificationCommand(Guid Id) : ICommand<Result>;
