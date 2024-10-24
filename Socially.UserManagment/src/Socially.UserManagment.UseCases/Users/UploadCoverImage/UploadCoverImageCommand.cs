using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.UploadCoverImage;
public record UploadCoverImageCommand(IFormFile File, Guid Id) : ICommand<Result<string>>;
