using System;
using PAX.TaskManager.Core;
using PAX.TaskManager.Core.Dependency;
using PAX.TaskManager.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PAX.TaskManager.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}