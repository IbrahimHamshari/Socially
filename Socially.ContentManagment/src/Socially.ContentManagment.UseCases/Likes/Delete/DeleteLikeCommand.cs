using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Likes.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Likes.Delete;
public record DeleteLikeCommand(DeleteLikeDto deleteLikeDto) : ICommand<Result>;
