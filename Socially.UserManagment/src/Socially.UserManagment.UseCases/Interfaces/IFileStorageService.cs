using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.UseCases.Interfaces;
public interface IFileStorageService
{
  Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName);
  Task DeleteFileAsync(string fileName, string bucketName);

}
