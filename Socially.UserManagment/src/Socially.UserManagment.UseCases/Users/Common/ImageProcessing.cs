using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Socially.UserManagment.UseCases.Users.Common;

public static class ImageProcessing
{
  public async static Task<byte[]> CompressImage(
      IFormFile imageFile,
      int? targetWidth = null, // Optional width
      int? targetHeight = null, // Optional height
      long maxAllowedSize = 1 * 1024 * 1024 // 1 MB

  )
  {
    // Validate the file format by checking the extension
    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
    {
      throw new NotSupportedException("Only .jpg, .jpeg, and .png files are allowed.");
    }

    // Step 1: Detect the image format using DetectFormatAsync
    var format = await Image.DetectFormatAsync(imageFile.OpenReadStream());

    // Step 2: Load the image
    using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
    {
      // Step 3: Crop or resize the image based on the provided target dimensions
      if (targetWidth.HasValue && targetHeight.HasValue)
      {
        // Crop the image if it's larger than the target size
        if (image.Width > targetWidth || image.Height > targetHeight)
        {
          image.Mutate(x => x.Resize(new ResizeOptions
          {
            Size = new Size(targetWidth.Value, targetHeight.Value),
            Mode = ResizeMode.Crop // Crop to fit the target size
          }));
        }
        // Resize the image if it's smaller than the target size
        else if (image.Width < targetWidth || image.Height < targetHeight)
        {
          image.Mutate(x => x.Resize(new ResizeOptions
          {
            Size = new Size(targetWidth.Value, targetHeight.Value),
            Mode = ResizeMode.Stretch 
          }));
        }
      }

      using (var compressedStream = new MemoryStream())
      {
        // Step 4: Get the encoder based on the detected format
        IImageEncoder encoder = GetEncoderByFormat(format);

        // Step 5: Save the compressed image to the MemoryStream using the appropriate encoder
        await image.SaveAsync(compressedStream, encoder);

        // Step 6: Check if the compressed size exceeds the maximum allowed size (1 MB)
        if (compressedStream.Length > maxAllowedSize)
        {
          throw new InvalidOperationException("Compressed image size exceeds the maximum allowed size of 1 MB.");
        }

        // Return the compressed image as a byte array
        return compressedStream.ToArray();
      }
    }
  }

  // Helper method to get the appropriate encoder based on the image format
  private static IImageEncoder GetEncoderByFormat(IImageFormat format)
  {
    if (format.Name.Equals("JPEG", StringComparison.OrdinalIgnoreCase))
    {
      return new JpegEncoder { Quality = 75 }; // JPEG compression with 75% quality
    }
    else if (format.Name.Equals("PNG", StringComparison.OrdinalIgnoreCase))
    {
      return new PngEncoder(); // PNG is lossless, so no quality settings
    }
    else
    {
      throw new NotSupportedException($"Image format '{format.Name}' is not supported.");
    }
  }
}
