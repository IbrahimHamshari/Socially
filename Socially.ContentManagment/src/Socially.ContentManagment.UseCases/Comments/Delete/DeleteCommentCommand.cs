using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Comments.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Comments.Delete;
public record DeleteCommentCommand(DeleteCommentDto deleteCommentDto) : ICommand<Result>;
