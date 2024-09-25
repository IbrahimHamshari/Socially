using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Update;
public record UpdateUserCommand(Guid Id, UserUpdateDto User) : ICommand<Result<UserUpdateDto>>;
