using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Socially.ContentManagment.Infrastructure.Messaging;

public class RabbitMqConsumerService : IRabbitMqConsumerService
{
  private readonly IConnection _connection;
  private readonly IModel _channel;
  private readonly RabbitMqConfiguration _config;

  public RabbitMqConsumerService(RabbitMqConfiguration config)
  {
    _config = config;

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

  public void StartListening()
  {
    var consumer = new EventingBasicConsumer(_channel);
    consumer.Received += (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      HandleMessage(message);
    };

    _channel.BasicConsume(queue: _config.QueueName,
                          autoAck: true,
                          consumer: consumer);

    Console.WriteLine($"Listening for messages on {_config.QueueName}...");
  }

  private void HandleMessage(string message)
  {
    // Process the received message
    Console.WriteLine($"Received message: {message}");
    // Add your custom logic here to handle the message
  }

  public void Dispose()
  {
    _channel?.Close();
    _connection?.Close();
  }
}
