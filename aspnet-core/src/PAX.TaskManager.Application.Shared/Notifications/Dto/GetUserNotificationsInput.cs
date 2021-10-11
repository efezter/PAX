using System;
using Abp.Notifications;
using PAX.TaskManager.Dto;

namespace PAX.TaskManager.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}