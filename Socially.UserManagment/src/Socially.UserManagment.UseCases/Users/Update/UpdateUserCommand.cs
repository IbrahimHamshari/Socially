using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.Update;
public record UpdateUserCommand(Guid Id, UserUpdateDto User) : ICommand<Result<UserUpdateDto>>;
