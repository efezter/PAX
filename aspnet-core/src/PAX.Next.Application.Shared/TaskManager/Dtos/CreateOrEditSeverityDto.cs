using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditSeverityDto : EntityDto<int?>
    {

        [Required]
        [StringLength(SeverityConsts.MaxNameLength, MinimumLength = SeverityConsts.MinNameLength)]
        public string Name { get; set; }

        public string IconUrl { get; set; }

        public int Order { get; set; }

    }
}