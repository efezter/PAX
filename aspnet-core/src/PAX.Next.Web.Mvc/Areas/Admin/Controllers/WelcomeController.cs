using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.Web.Controllers;

namespace PAX.Next.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize]
    public class WelcomeController : NextControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}