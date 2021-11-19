using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetCommentForEditOutput
    {
        public CreateOrEditCommentDto Comment { get; set; }

        public string PaxTaskHeader { get; set; }

        public string UserName { get; set; }

    }
}