using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.UserAggregate;

namespace Socially.ContentManagment.UseCases.Users.ForgetPassword;

public class PasswordForgotEvent(User _user) : DomainEventBase
{
  public User User { get; private set; } = _user;
}
