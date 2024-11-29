namespace Socially.Messaging.Infrastructure.Messaging;

public class RabbitMqConfiguration
{
  public required string Hostname { get; set; }
  public required string QueueName { get; set; }
  public required string UserName { get; set; }
  public required string Password { get; set; }
  public required int Port { get; set; }
  public required string VirtualHost { get; set; }
  public required bool Enabled { get; set; }
}
