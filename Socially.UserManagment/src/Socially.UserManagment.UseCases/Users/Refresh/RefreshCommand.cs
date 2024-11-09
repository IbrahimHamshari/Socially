using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.UseCases.Users.Refresh;

public record RefreshCommand(string refreshToken) : ICommand<Result<string[]>>;
