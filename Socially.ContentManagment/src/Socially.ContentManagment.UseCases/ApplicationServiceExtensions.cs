using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Services;
using Socially.ContentManagment.UseCases.Services;

namespace Socially.ContentManagment.UseCases;
public static class ApplicationServiceExtensions
{
  public static IServiceCollection addApplicationServices(this IServiceCollection services,
    ILogger _logger)
  {
    services.AddScoped(typeof(IDeletePostService), typeof(DeletePostService));
    services.AddScoped(typeof(ICreatePostService), typeof(CreatePostService));
    services.AddScoped(typeof(IMediaUploadService), typeof(MediaUploadService));
    return services;
  }
}
