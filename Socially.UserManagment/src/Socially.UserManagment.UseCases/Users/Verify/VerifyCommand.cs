using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.UseCases.Users.Verify;
public record VerifyCommand(string Token) : ICommand<Result>;
