using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Login;

public record LoginCommand(UserLoginDto User) : ICommand<Result<Tokens>>;
