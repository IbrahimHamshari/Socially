using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Get;
public record GetUserQuery(Guid Id) : IQuery<Result<UserDto>>;
