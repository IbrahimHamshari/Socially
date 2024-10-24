using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Socially.ContentManagment.Infrastructure.Messaging;

public class RabbitMqService : IRabbitMqService
{
  private readonly RabbitMqConfiguration _config;
  private readonly IConnection _connection;
  private readonly IModel _channel;

  public RabbitMqService(IOptions<RabbitMqConfiguration> config)
  {
    _config = config.Value;

    if (_config.Enabled)
    {
      var factory = new ConnectionFactory
      {
        HostName = _config.Hostname,
        UserName = _config.UserName,
        Password = _config.Password,
        Port = _config.Port,
        VirtualHost = _config.VirtualHost
      };

      _connection = factory.CreateConnection();
      _channel = _connection.CreateModel();
    }
    else
    {
      throw new InvalidOperationException("RabbitMQ is disabled in configuration.");
    }
  }

  public void PublishMessage(string message)
  {
    if (!_config.Enabled)
    {
      throw new InvalidOperationException("RabbitMQ is disabled in configuration.");
    }

    var body = Encoding.UTF8.GetBytes(message);
    _channel.BasicPublish(exchange: "",
                          routingKey: _config.QueueName,
                          basicProperties: null,
                          body: body);

    Console.WriteLine($"[x] Sent {message}");
  }

  public void Dispose()
  {
    _channel.Close();
    _connection.Close();
  }
}
