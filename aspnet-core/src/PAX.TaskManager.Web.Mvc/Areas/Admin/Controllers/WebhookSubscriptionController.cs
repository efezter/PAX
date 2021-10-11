using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Webhooks;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Authorization;
using PAX.TaskManager.Web.Areas.Admin.Models.Webhooks;
using PAX.TaskManager.Web.Controllers;
using PAX.TaskManager.WebHooks;

namespace PAX.TaskManager.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_WebhookSubscription)]
    public class WebhookSubscriptionController : TaskManagerControllerBase
    {
        private readonly IWebhookSubscriptionAppService _webhookSubscriptionAppService;
        private readonly IWebhookEventAppService _webhookEventAppService;

        public WebhookSubscriptionController(
            IWebhookSubscriptionAppService webhookSubscriptionAppService,
            IWebhookEventAppService webhookEventAppService
            )
        {
            _webhookSubscriptionAppService = webhookSubscriptionAppService;
            _webhookEventAppService = webhookEventAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_WebhookSubscription_Edit)]
        public async Task<IActionResult> EditModal(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentException(nameof(subscriptionId));
            }

            var availableWebhooks = await _webhookSubscriptionAppService.GetAllAvailableWebhooks();
            var model = new CreateOrEditWebhookSubscriptionViewModel()
            {
                WebhookSubscription = await _webhookSubscriptionAppService.GetSubscription(subscriptionId),
                AvailableWebhookEvents = availableWebhooks
            };

            return PartialView("_CreateOrEditModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_WebhookSubscription_Create)]
        public async Task<IActionResult> CreateModal()
        {
            var availableWebhooks = await _webhookSubscriptionAppService.GetAllAvailableWebhooks();
            var model = new CreateOrEditWebhookSubscriptionViewModel()
            {
                WebhookSubscription = new WebhookSubscription(),
                AvailableWebhookEvents = availableWebhooks
            };

            return PartialView("_CreateOrEditModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_WebhookSubscription_Detail)]
        public async Task<IActionResult> Detail(string id)
        {
            var subscription = await _webhookSubscriptionAppService.GetSubscription(id);

            return View(subscription);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_WebhookSubscription_Create)]
        public async Task<IActionResult> WebHookEventDetail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(nameof(id));
            }

            var webhookEvent = await _webhookEventAppService.Get(id);
            return View(webhookEvent);
        }
    }
}