using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ardalis.SharedKernel;
using Ardalis.SmartEnum;

namespace Socially.ContentManagment.Core.PostAggregate;
public class Privacy : SmartEnum<Privacy>
{
  public static readonly Privacy Public = new(nameof(Public), 1);
  public static readonly Privacy Friends = new(nameof(Friends), 2);
  public static readonly Privacy Private = new(nameof(Private), 3);
  protected Privacy(string name, int value) : base(name, value){}
}
