using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.ChangePassword;
public record ChangePasswordCommand(Guid id, ChangePasswordDto passwords) : ICommand<Result>;
