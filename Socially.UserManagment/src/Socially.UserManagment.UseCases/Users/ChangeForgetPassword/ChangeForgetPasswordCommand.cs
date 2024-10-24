using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Users.Common;

namespace Socially.ContentManagment.UseCases.Users.ChangePasswordForget;
public record ChangeForgetPasswordCommand(string Token, string NewPassword) : ICommand<Result<Tokens>>;
