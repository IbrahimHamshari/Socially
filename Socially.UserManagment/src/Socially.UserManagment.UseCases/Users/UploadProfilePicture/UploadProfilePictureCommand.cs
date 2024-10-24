using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace Socially.ContentManagment.UseCases.Users.UploadProfilePicture;
public record UploadProfilePictureCommand(IFormFile File, Guid Id) : ICommand<Result<string>>;
