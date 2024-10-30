using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Socially.ContentManagment.UseCases.Interfaces;
public interface IMediaUploadService
{
  public Task<string> UploadMediaAsync(IFormFile? media, string existMediaURL = "");
}
