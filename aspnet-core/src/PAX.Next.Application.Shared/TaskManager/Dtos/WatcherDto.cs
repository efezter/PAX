using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class WatcherDto : EntityDto
    {

        public int PaxTaskId { get; set; }

        public long UserId { get; set; }

    }
}