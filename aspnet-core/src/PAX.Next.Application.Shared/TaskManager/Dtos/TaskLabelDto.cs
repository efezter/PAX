using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class TaskLabelDto : EntityDto
    {

        public int PaxTaskId { get; set; }

        public int LabelId { get; set; }

    }
}