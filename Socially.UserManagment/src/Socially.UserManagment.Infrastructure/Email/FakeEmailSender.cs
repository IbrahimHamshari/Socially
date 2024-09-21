using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;

namespace Socially.UserManagment.Infrastructure.Email;
public class FakeEmailSender(ILogger<FakeEmailSender> _logger,
  MailserverConfiguration _mailServerConfiguration) : IEmailSender
{
  public Task SendEmailAsync(string to, string subject, string body)
  {
    _logger.LogInformation("Not actually sending an email to {to} from {from} with subject {subject}", to, _mailServerConfiguration.Username, subject);
    return Task.CompletedTask;
  }
}
