using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.UseCases.Users.ForgetPassword;
public record ForgetPasswordCommand(string Email) : ICommand<Result>;
