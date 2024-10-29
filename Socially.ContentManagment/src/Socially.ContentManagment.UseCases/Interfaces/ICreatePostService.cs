using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Interfaces;
public interface ICreatePostService
{
  Task<Result<PostDto>> CreatePost(Guid userId, string content, int privacy, string mediaURL);

}
