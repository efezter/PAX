﻿using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using System.Collections.Generic;

namespace PAX.Next.TaskManager
{
    public interface ILabelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLabelForViewDto>> GetAll(GetAllLabelsInput input);

        Task<IEnumerable<LabelDto>> GetLabelsByTaskId(int taskId);

        Task<GetLabelForEditOutput> GetLabelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLabelDto input);

        Task Delete(int Id);

    }
}