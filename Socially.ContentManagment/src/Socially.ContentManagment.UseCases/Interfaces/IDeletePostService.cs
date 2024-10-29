using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace Socially.ContentManagment.UseCases.Interfaces;
public interface IDeletePostService
{
  Task<Result> DeletePost(Guid post);
}
