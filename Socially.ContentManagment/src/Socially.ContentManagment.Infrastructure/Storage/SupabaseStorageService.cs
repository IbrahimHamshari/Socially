using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Socially.ContentManagment.UseCases.Interfaces;
using Supabase;
using Supabase.Storage.Interfaces;

namespace Socially.ContentManagment.Infrastructure.Storage;
public class SupabaseStorageService : IFileStorageService
{
  private readonly Client _storageClient;

  public SupabaseStorageService(Client storageClient)
  {
    _storageClient = storageClient;
  }

  public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName)
  {
    var memoryStream = new MemoryStream();
    fileStream.CopyTo(memoryStream);
    var storage = _storageClient.Storage.From(bucketName);
    await storage.Upload(memoryStream.ToArray(), fileName);
    var fileUrl = storage.GetPublicUrl(fileName);
    return fileUrl;
  }

  public async Task DeleteFileAsync(string fileName, string bucketName)
  {
    var storage = _storageClient.Storage.From(bucketName);
    await storage.Remove(new List<string> { fileName });
  }

  public async Task DeleteFileByUrlAsync(string fileUrl, string bucketName)
  {
    // Extract the file name from the URL
    var uri = new Uri(fileUrl);
    var fileName = HttpUtility.UrlDecode(uri.Segments.Last());

    // Delete the file using the extracted file name
    await DeleteFileAsync(fileName, bucketName);
  }
}
