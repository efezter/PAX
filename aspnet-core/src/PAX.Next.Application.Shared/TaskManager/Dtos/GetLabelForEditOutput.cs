using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetLabelForEditOutput
    {
        public CreateOrEditLabelDto Label { get; set; }

    }
}