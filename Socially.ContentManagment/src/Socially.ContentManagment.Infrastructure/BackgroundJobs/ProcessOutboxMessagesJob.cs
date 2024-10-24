using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Quartz;
using Socially.ContentManagment.Infrastructure.Data;

namespace Socially.ContentManagment.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob
{

}
