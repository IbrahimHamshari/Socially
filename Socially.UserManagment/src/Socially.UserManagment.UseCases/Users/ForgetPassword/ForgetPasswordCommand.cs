using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.UseCases.Users.ForgetPassword;
public record ForgetPasswordCommand(string Email) : ICommand<Result>;
