using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class HistoryDto
    {
        public string ChangeText { get; set; }

        public string UserName { get; set; }

        public DateTime CreationTime { get; set; }

    }
}