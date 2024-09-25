using Ardalis.SharedKernel;
using Socially.UserManagment.Core.UserAggregate;

namespace Socially.UserManagment.UseCases.Users.ForgetPassword;

public class PasswordForgotEvent(User _user) : DomainEventBase
{
  public User User { get; private set; } = _user;
}
