using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditPaxTaskAttachmentDto : EntityDto<int?>
    {

        public int PaxTaskId { get; set; }

        public string FileName { get; set; }

    }
}