using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Web.Controllers;

namespace PAX.TaskManager.Web.Public.Controllers
{
    public class HomeController : TaskManagerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}