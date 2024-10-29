using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Utils;

namespace Socially.ContentManagment.UseCases.Posts.Services;
public  class CreatePostService(IRepository<Post> _repository) : ICreatePostService
{
  public async Task<Result<PostDto>> CreatePost(Guid userId, string content, int privacy, string mediaURL)
  {
    Privacy pr = PrivacyConversions.IntToPrivacy(privacy);
    Post newPost = new Post ( Guid.NewGuid(), userId, content, pr, mediaURL);
    await _repository.AddAsync(newPost);
    return Result.Created(new PostDto { Id = newPost.Id, Content = newPost.Content, Privacy = newPost.Privacy, UserId = newPost.UserID, MediaURL = newPost.MediaURL});
    
  }
}
