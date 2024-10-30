using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Delete;
public record DeletePostCommand(Guid Id, Guid UserId) : ICommand<Result<PostDto>>;
