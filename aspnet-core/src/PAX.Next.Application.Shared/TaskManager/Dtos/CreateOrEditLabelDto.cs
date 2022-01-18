using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditLabelDto : EntityDto<int?>
    {
        public string Name { get; set; }
    }
}