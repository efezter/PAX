using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetPaxTaskAttachmentForEditOutput
    {
        public CreateOrEditPaxTaskAttachmentDto PaxTaskAttachment { get; set; }

        public string PaxTaskHeader { get; set; }

    }
}