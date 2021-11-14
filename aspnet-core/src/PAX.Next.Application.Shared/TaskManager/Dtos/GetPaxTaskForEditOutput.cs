using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetPaxTaskForEditOutput
    {
        public CreateOrEditPaxTaskDto PaxTask { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public string SeverityName { get; set; }

        public string TaskStatusName { get; set; }

    }
}