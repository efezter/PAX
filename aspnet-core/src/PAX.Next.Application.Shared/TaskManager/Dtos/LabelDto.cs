using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class LabelDto : EntityDto
    {
        public int TaskLabelId { get; set; }
        public string Name { get; set; }
    }
}