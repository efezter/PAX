using Abp.AspNetCore.Mvc.Authorization;
using PAX.TaskManager.Authorization;
using PAX.TaskManager.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace PAX.TaskManager.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}