using Abp.AspNetCore.Mvc.Authorization;
using PAX.Next.Authorization;
using PAX.Next.Storage;
using Abp.BackgroundJobs;

namespace PAX.Next.Web.Controllers
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