using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate.Events;

namespace Socially.UserManagment.UseCases.Users.Handlers;
public class RecoverAccountEventHandler(
  IEmailSender _emailSender,
  ILogger<RecoverAccountEventHandler> _logger) : INotificationHandler<AccountRecoveredEvent>
{
  public async Task Handle(AccountRecoveredEvent notification, CancellationToken cancellationToken)
  {
    var user = notification.User;
    var username = user.Username;
    var email = user.Email;
    var changeTime = DateTimeOffset.UtcNow;
    var id = user.Id;
    var body = $@"
    Hi {username},

    This is a confirmation that your password was successfully changed on {changeTime:MMMM dd, yyyy} at {changeTime:HH:mm} (UTC), following your password reset request.

    If you initiated this change, no further action is required.

    If you did **not** request a password reset or believe this was done in error, please contact our support team immediately so we can secure your account.

    For enhanced security, we recommend enabling two-factor authentication (2FA) if you haven’t done so already.

    Best regards,
    The Socially Team

";
    var subject = "Sucssefully changing the password";
    await _emailSender.SendEmailAsync(email, subject, body);
    _logger.LogInformation("Sucessfully Changed Password to the user with id of {id}", id);

  }
}
