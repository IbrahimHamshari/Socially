using MediatR;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate.Events;

namespace Socially.UserManagment.UseCases.Users.Handlers;

public class UserChangedPasswordEventHandler(IEmailSender _emailSender,
  ILogger<UserChangedPasswordEventHandler> _logger) : INotificationHandler<UserChangedPasswordEvent>
{
  public async Task Handle(UserChangedPasswordEvent notification, CancellationToken cancellationToken)
  {
    var user = notification.User;
    var email = user.Email;
    var id = user.Id;
    var changeTime = DateTimeOffset.UtcNow;
    var body = $@"
    Hi {user.Username},

    Your password was successfully changed on {changeTime:MMMM dd, yyyy} at {changeTime:HH:mm} (UTC).

    If you did not make this change, please contact our support team immediately to secure your account.

    Best regards,
    The Socially Team
    ";
    var subject = "Password Changed";
    await _emailSender.SendEmailAsync(email, subject, body);
    _logger.LogInformation("Sucessfully Changed Password to the user with id of {id}", id);
  }
}
