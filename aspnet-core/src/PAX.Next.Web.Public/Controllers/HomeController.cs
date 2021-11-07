using Microsoft.AspNetCore.Mvc;
using PAX.Next.Web.Controllers;

namespace PAX.Next.Web.Public.Controllers
{
    public class HomeController : NextControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}