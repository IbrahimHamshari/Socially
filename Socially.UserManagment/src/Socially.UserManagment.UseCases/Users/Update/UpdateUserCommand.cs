using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Update;
public record UpdateUserCommand(Guid Id, UserUpdateDto User) : ICommand<Result<UserUpdateDto>>;
