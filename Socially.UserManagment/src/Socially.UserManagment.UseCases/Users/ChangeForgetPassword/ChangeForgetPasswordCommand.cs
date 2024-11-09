using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common;

namespace Socially.UserManagment.UseCases.Users.ChangePasswordForget;
public record ChangeForgetPasswordCommand(string Token, string NewPassword) : ICommand<Result<Tokens>>;
