using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public string ChangeText { get; set; }

        public DateTime CreationTime { get; set; }

    }
}