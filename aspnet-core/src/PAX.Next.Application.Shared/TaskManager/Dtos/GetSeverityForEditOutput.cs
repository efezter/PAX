using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetSeverityForEditOutput
    {
        public CreateOrEditSeverityDto Severity { get; set; }

    }
}