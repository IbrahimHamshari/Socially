using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Register;
public record RegisterUserCommand(UserRegistrationDto User) : ICommand<Result<Guid>>;
