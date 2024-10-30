using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Update;
public record UpdatePostCommand(UpdatePostDto updatePostDto, Guid userId) : ICommand<Result<PostDto>>;
