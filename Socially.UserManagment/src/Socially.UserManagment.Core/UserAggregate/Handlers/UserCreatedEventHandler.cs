using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.Interfaces;
using Socially.ContentManagment.Core.UserAggregate.Events;

namespace Socially.ContentManagment.UseCases.Users.Handlers;

public class UserCreatedEventHandler(IEmailSender _emailService,
  IHttpContextAccessor _httpContextAccessor,
  ILogger<UserCreatedEventHandler> _logger) : INotificationHandler<UserCreatedEvent>
{
  public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    var user = domainEvent.User;
    var body = $@"
    Hi {user.Username},

    Welcome to Socially! We're excited to have you join our community.

    To complete your registration, please confirm your email address by clicking the link below:

    {$"https://{_httpContextAccessor.HttpContext!.Request.Host.Value}/{user.VerificationToken}"}

    If the link doesn't work, you can copy and paste it into your browser.

    Please note that the link is only available for 3 hours.

    Best regards,
    The Socially Team
    ";
    var subject = "Account Registeration";
    await _emailService.SendEmailAsync(user.Email, subject, body);
    _logger.LogInformation("a Register Email has been sent to the user with Id of {id}", user.Id);
  }
}
