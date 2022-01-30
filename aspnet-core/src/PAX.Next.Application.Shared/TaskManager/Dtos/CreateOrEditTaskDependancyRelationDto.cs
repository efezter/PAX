using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditTaskDependancyRelationDto : EntityDto<int?>
    {

        public int PaxTaskId { get; set; }

        public int DependOnTaskId { get; set; }

    }
}