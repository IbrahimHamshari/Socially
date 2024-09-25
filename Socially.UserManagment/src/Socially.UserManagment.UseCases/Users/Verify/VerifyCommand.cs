using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.UseCases.Users.Verify;
public record VerifyCommand(string Token) : ICommand<Result>;
