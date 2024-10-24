using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.Register;
public record RegisterUserCommand(UserRegistrationDto User) : ICommand<Result<Guid>>;
