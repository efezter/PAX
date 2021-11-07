using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetTaskStatusForEditOutput
    {
        public CreateOrEditTaskStatusDto TaskStatus { get; set; }

    }
}