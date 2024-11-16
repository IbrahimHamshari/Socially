using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socially.ContentManagment.Core.PostAggregate;

namespace Socially.ContentManagment.UseCases.Posts.Utils;
public static class PrivacyConversions
{
  public static Privacy IntToPrivacy(int privacy)
  {
    Privacy pr = Privacy.Public;
    switch (privacy)
    {
      case 1: pr = Privacy.Public; break;
      case 2: pr = Privacy.Friends; break;
      case 3: pr = Privacy.Private; break;
    }
    return pr;
  }
}
