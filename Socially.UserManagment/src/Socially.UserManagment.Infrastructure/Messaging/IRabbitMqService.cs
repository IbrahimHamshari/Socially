namespace Socially.ContentManagment.Infrastructure.Messaging;

public interface IRabbitMqService : IDisposable
{
  void PublishMessage(string message);
}
