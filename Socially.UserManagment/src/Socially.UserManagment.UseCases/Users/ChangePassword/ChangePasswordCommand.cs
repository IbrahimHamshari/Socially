using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.ChangePassword;
public record ChangePasswordCommand(Guid id, ChangePasswordDto passwords) : ICommand<Result>;
