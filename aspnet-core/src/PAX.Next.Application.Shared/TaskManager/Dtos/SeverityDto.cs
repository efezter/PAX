using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class SeverityDto : EntityDto
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public DateTime InsertedDate { get; set; }

    }
}