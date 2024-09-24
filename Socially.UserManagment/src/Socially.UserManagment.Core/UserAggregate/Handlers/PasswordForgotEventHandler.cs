using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.ForgetPassword;

namespace Socially.UserManagment.UseCases.Users.Handlers;
public class PasswordForgotEventHandler(IEmailSender _emailSender,
  IHttpContextAccessor _httpContextAccessor,
  ILogger<PasswordForgotEventHandler> _logger) : INotificationHandler<PasswordForgotEvent>
{


  public async Task Handle(PasswordForgotEvent notification, CancellationToken cancellationToken)
  {
    var user = notification.User;
    var username = user.Username;
    var requestTime = DateTimeOffset.UtcNow;
    var email = user.Email;
    var resetToken = user.ResetPasswordToken;
    var id = user.Id;
    var body = $@"
    Hi {username},

    We received a request to reset your password on {requestTime:MMMM dd, yyyy} at {requestTime:HH:mm} (UTC).

    If you requested this password reset, please click the link below to proceed with resetting your password:

    {$"https://{_httpContextAccessor.HttpContext!.Request.Host}/{resetToken}"}

    If you did not request a password reset, please ignore this email. For additional help, feel free to contact our support team.

    Please note that the link is only available for 3 hours.

    Best regards,
    The Socially Team
    ";
    var subject = "Reset Account Password";
    await _emailSender.SendEmailAsync(email, subject, body);
    _logger.LogInformation("Sucessfully Changed Password to the user with id of {id}", id);
  }
}
