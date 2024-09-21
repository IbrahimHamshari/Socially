using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Socially.UserManagment.Core.Interfaces;

namespace Socially.UserManagment.Infrastructure.Email;
public class MimeKitEmailSender : IEmailSender
{
  private readonly ILogger<MimeKitEmailSender> _logger;
  private readonly MailserverConfiguration _mailserverConfiguration;

  public MimeKitEmailSender(ILogger<MimeKitEmailSender> logger,
    IOptions<MailserverConfiguration> mailserverOptions)
  {
    _logger = logger;
    _mailserverConfiguration = mailserverOptions.Value!;
  }


  public async Task SendEmailAsync(string to, string subject, string body)
  {

    using var client = new SmtpClient();
    await client.ConnectAsync(_mailserverConfiguration.Hostname,
      _mailserverConfiguration.Port, 
      MailKit.Security.SecureSocketOptions.StartTls);
    await client.AuthenticateAsync(_mailserverConfiguration.Username, _mailserverConfiguration.Password);

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress(_mailserverConfiguration.Username, _mailserverConfiguration.Username));
    message.To.Add(new MailboxAddress(to, to));
    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };

    await client.SendAsync(message);

    await client.DisconnectAsync(true,
      new CancellationToken(canceled: true));

    _logger.LogWarning("Sent email to {to} with subject {subject} using {type}.", to, subject, this.ToString());

  }
}
