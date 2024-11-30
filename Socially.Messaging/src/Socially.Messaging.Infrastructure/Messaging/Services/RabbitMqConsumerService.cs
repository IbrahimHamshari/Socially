using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Socially.Messaging.Infrastructure.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Messaging.Infrastructure.Messaging.Services;

public class RabbitMqConsumerService : IRabbitMqConsumerService
{
  private readonly IConnection _connection;
  private readonly IModel _channel;
  private readonly RabbitMqConfiguration _config;

  public RabbitMqConsumerService(IOptions<RabbitMqConfiguration> config)
  {
    _config = config.Value;

    if (_config.Enabled)
    {
      var factory = new ConnectionFactory()
      {
        HostName = _config.Hostname,
        Port = _config.Port,
        UserName = _config.UserName,
        Password = _config.Password,
        VirtualHost = _config.VirtualHost
      };

      _connection = factory.CreateConnection();
      _channel = _connection.CreateModel();

      // Declare the queue if it doesn't exist
      _channel.QueueDeclare(queue: _config.QueueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
    }
    else
    {
      throw new InvalidOperationException("RabbitMQ is disabled in configuration.");
    }

  }

  public async Task<string> ConsumeMessageAsync()
  {
    var tcs = new TaskCompletionSource<string>();

    var consumer = new EventingBasicConsumer(_channel);
    consumer.Received += (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      tcs.SetResult(message);
    };

    _channel.BasicConsume(queue: _config.QueueName,
                          autoAck: true,
                          consumer: consumer);

    // Wait for a message to be consumed
    return await tcs.Task;
  }

  public void Dispose()
  {
    _channel?.Close();
    _connection?.Close();
  }
}
