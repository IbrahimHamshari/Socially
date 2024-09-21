using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.UseCases.Users.ForgetPassword;
public class PasswordForgotEvent(User _user) : DomainEventBase
{
  public User User { get; private set; } = _user;
}

