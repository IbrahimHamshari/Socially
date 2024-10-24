using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Users.Common;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.Login;

public record LoginCommand(UserLoginDto User) : ICommand<Result<Tokens>>;
