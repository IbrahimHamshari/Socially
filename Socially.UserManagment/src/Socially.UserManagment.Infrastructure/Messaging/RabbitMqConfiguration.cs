using RabbitMQ.Client;
using System;
using System.Text;

namespace Socially.ContentManagment.Infrastructure.Messaging;


public class RabbitMqConfiguration
{
  public required string Hostname { get; set; }
  public required string QueueName { get; set; }
  public required string UserName { get; set; }
  public required string Password { get; set; }
  public required int Port { get; set; }
  public required string VirtualHost { get; set; }
  public required bool Enabled { get; set; }

  public override string ToString()
  {
    return $"RabbitMQConfiguration:" +
      $"HostName: {Hostname}" +
      $"QueueName: {QueueName}" +
      $"UserName: {UserName}" +
      $"Password: {Password}" +
      $"Port: {Port}" +
      $"VirtualHost: {VirtualHost}" +
      $"Enabled: {Enabled}";
  }
}
