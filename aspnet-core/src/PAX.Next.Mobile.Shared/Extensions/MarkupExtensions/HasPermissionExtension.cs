using System;
using PAX.Next.Core;
using PAX.Next.Core.Dependency;
using PAX.Next.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PAX.Next.Extensions.MarkupExtensions
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