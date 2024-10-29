using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Shares.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Shares.Create;
public record SharePostCommand(SharePostDto sharePostDto) : ICommand<Result>;
