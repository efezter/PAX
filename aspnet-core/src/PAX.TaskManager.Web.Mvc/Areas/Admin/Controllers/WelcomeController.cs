using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Web.Controllers;

namespace PAX.TaskManager.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize]
    public class WelcomeController : TaskManagerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}