using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Likes.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Likes.Create;
public record ToggleLikeCommand(ToggleLikeDto createLikeDto, Guid userId) : ICommand<Result>;
