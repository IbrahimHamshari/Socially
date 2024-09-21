using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate.Events;

namespace Socially.UserManagment.UseCases.Users.Handlers;
public record UserVerifiedEventHandler(IEmailSender _emailService,
  ILogger<UserVerifiedEventHandler> _logger) : INotificationHandler<UserVerifiedEvent>
{
  public async Task Handle(UserVerifiedEvent notification, CancellationToken cancellationToken)
  {
    var user = notification.User;
    var body = $@"
    Hi {user.Username},

    Congratulations! Your account has been successfully verified.

    You can now log in to your account and start using all the features of our platform.

    If you have any questions or encounter any issues, feel free to contact our support team.

    Best regards,
    The Socially Team
    ";
    var subject = "Account Verification";
    await _emailService.SendEmailAsync(user.Email, subject, body);
    _logger.LogInformation("a Register Email has been sent to the user with Id of {id}", user.Id);
  }
  
}
