using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Shares.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Shares.Delete;
public record DeleteShareCommand(SharePostDto sharePostDto, Guid userId) : ICommand<Result>;
