﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetWatcherForEditOutput
    {
        public CreateOrEditWatcherDto Watcher { get; set; }

        public string PaxTaskHeader { get; set; }

        public string UserName { get; set; }

    }
}