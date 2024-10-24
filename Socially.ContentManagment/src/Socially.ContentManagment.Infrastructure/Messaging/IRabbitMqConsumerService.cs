using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.Infrastructure.Messaging;
public interface IRabbitMqConsumerService : IDisposable
{
  void StartListening();

}
