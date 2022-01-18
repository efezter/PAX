using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditTaskLabelDto : EntityDto<int?>
    {

        public int PaxTaskId { get; set; }

        public int LabelId { get; set; }

    }
}