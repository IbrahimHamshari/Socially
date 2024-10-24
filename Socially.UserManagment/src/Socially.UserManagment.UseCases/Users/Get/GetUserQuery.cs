using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.Get;
public record GetUserQuery(Guid Id) : IQuery<Result<UserDto>>;
