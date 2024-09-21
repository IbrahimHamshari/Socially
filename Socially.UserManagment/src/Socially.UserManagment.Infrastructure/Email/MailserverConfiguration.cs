namespace Socially.UserManagment.Infrastructure.Email;

public class MailserverConfiguration()
{
  public string Hostname { get; set; } = "localhost";
  public int Port { get; set; } = 25;
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public bool UseSSL { get; set; } = true; 
}
