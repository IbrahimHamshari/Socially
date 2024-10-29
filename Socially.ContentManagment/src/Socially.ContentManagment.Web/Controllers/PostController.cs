using Microsoft.AspNetCore.Mvc;

namespace Socially.ContentManagment.Web.Controllers;
public class PostController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}
