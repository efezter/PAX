using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class TaskDependancyRelationDto : EntityDto
    {

        public int PaxTaskId { get; set; }

        public int DependOnTaskId { get; set; }

        public string DependOnHeader { get; set; }

    }
}