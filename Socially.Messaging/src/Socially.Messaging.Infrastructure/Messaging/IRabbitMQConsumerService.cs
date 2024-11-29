﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Messaging.Infrastructure.Messaging;
public interface IRabbitMqConsumerService : IDisposable
{
  public Task<string> ConsumeMessageAsync();

}