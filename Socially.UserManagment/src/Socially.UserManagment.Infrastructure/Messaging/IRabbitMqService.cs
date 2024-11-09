namespace Socially.UserManagment.Infrastructure.Messaging;

public interface IRabbitMqService : IDisposable
{
  void PublishMessage(string message);
}
