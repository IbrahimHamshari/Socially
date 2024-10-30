using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Comments.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Comments.Update;
public record UpdateCommentCommand(UpdateCommentDto updateCommentDto, Guid userId) : ICommand<Result>;
