using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetTaskLabelForEditOutput
    {
        public CreateOrEditTaskLabelDto TaskLabel { get; set; }

        public string PaxTaskHeader { get; set; }

        public string LabelName { get; set; }

    }
}