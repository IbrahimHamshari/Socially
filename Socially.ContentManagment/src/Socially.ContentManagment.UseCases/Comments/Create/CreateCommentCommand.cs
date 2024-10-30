using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Comments.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Comments.Create;
public record CreateCommentCommand(CreateCommentDto createCommandDto, Guid userId) : ICommand<Result>;
