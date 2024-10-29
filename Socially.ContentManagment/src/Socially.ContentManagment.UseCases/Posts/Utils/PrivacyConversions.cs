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
      case 0: privacy = Privacy.Public; break;
      case 1: privacy = Privacy.Private; break;
      case 2: privacy = Privacy.Friends; break;
    }
    return pr;
  }
}
