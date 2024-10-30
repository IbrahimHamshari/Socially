using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Socially.ContentManagment.UseCases.Constants;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Xabe.FFmpeg;
using static System.Net.Mime.MediaTypeNames;

namespace Socially.ContentManagment.UseCases.Posts.Create;

public class CreatePostCommandHandler(IFileStorageService _fileStorage, ICreatePostService _service)
    : ICommandHandler<CreatePostCommand, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
  {
    var mediaURL = "";

    if (request.PostDTO.Media != null)
    {
      var media = request.PostDTO.Media;

      // Check if the media is an image or video by examining the content type
      if (media.ContentType.StartsWith("image"))
      {
        // Compress and upload image
        using var compressedImageStream = await CompressImageAsync(media.OpenReadStream());
        mediaURL = await _fileStorage.UploadFileAsync(compressedImageStream,Guid.NewGuid().ToString("N"), BucketStorageConstants.POSTMEDIABUCKET);
      }
      else if (media.ContentType.StartsWith("video"))
      {
        // Compress and upload video
        using var compressedVideoStream = await CompressVideoAsync(media.OpenReadStream());
        mediaURL = await _fileStorage.UploadFileAsync(compressedVideoStream, Guid.NewGuid().ToString("N"), BucketStorageConstants.POSTMEDIABUCKET);
      }
    }

    return await _service.CreatePost(request.UserId, request.PostDTO.Content, request.PostDTO.Privacy, mediaURL);
  }

  private async Task<Stream> CompressImageAsync(Stream imageStream)
  {
    var outputStream = new MemoryStream();

    // Load image using ImageSharp's Image class
    using (var image = await SixLabors.ImageSharp.Image.LoadAsync(imageStream))
    {
      // Resize the image to a smaller size if needed (e.g., 1080px width max)
      image.Mutate(x => x.Resize(new ResizeOptions
      {
        Mode = ResizeMode.Max,
        Size = new Size(1080, 0) // Width set, height calculated to maintain aspect ratio
      }));

      // Save the image with a lower quality
      var encoder = new JpegEncoder { Quality = 50 };
      await image.SaveAsync(outputStream, encoder);
    }

    outputStream.Position = 0; // Reset stream position for uploading
    return outputStream;
  }


  public async Task<Stream> CompressVideoAsync(Stream videoStream)
  {
    var outputStream = new MemoryStream();

    var inputFilePath = Path.GetTempFileName();
    var outputFilePath = Path.GetTempFileName();

    await using (var fileStream = new FileStream(inputFilePath, FileMode.Create, FileAccess.Write))
    {
      await videoStream.CopyToAsync(fileStream);
    }

    // Ensure FFmpeg is downloaded and set up
    await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);

    // Create an input media file
    var inputFile = await FFmpeg.GetMediaInfo(inputFilePath);

    // Set up the conversion
    var conversion = FFmpeg.Conversions.New()
        .AddStream(inputFile.VideoStreams.FirstOrDefault()?.SetSize(VideoSize.Hd720))
        .SetOutput(outputFilePath)
        .SetOverwriteOutput(true); // Overwrite if file already exists

    // Start the conversion process
    await conversion.Start();

    // Load the compressed file into a stream
    await using (var fs = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read))
    {
      await fs.CopyToAsync(outputStream);
    }

    outputStream.Position = 0; // Reset stream position for uploading

    // Clean up temp files
    File.Delete(inputFilePath);
    File.Delete(outputFilePath);

    return outputStream;
  }

}
