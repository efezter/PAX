using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class TaskStatusDto : EntityDto
    {
        public string Name { get; set; }

        public string IconUrl { get; set; }
    }
}