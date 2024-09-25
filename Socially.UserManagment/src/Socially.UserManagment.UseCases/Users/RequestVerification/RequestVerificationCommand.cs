using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.UseCases.Users.RequestVerification;
public record RequestVerificationCommand(Guid Id) : ICommand<Result>;
