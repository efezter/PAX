using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetTaskDependancyRelationForEditOutput
    {
        public CreateOrEditTaskDependancyRelationDto TaskDependancyRelation { get; set; }

        public string PaxTaskHeader { get; set; }

        public string PaxTaskHeader2 { get; set; }

    }
}