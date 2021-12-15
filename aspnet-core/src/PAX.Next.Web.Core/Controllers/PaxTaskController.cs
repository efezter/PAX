using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PAX.Next.DemoUiComponents.Dto;
using PAX.Next.Storage;
using PAX.Next.TaskManager;
using System.IO;
using PAX.Next.TaskManager.Dtos;

namespace PAX.Next.Web.Controllers
{
    [AbpMvcAuthorize]
    public class PaxTaskController : NextControllerBase
    {
        private readonly PaxTaskAttachmentsAppService _paxTaskAttachmentsAppService;
        private readonly IHostEnvironment _env;

        public PaxTaskController(PaxTaskAttachmentsAppService paxTaskAttachmentsAppService, IHostEnvironment env)
        {
            _env = env;
            _paxTaskAttachmentsAppService = paxTaskAttachmentsAppService;
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles([FromQuery(Name = "taskId")] string taskId)
        {
            try
            {
                var files = Request.Form.Files;

                //Check input
                if (files == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                List<PaxTaskAttachmentDto> filesOutput = new List<PaxTaskAttachmentDto>();

                foreach (var file in files)
                {
                    if (file.Length > 1048576) //1MB
                    {
                        throw new UserFriendlyException(L("File_SizeLimit_Error"));
                    }

                    byte[] fileBytes;
                    using (var stream = file.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }



                    var attchUrl = "wwwroot/" + AbpSession.TenantId + "/Tasks/"+ taskId + "/Attachments";

                    var dir = Path.Combine(_env.ContentRootPath, attchUrl);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    var filePath = Path.Combine(dir, file.FileName);

                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    CreateOrEditPaxTaskAttachmentDto attchDto = new CreateOrEditPaxTaskAttachmentDto();
                    attchDto.PaxTaskId = int.Parse(taskId);
                    attchDto.FileName = filePath;

                   int insertedId = await _paxTaskAttachmentsAppService.CreateOrEdit(attchDto);

                    filesOutput.Add(new PaxTaskAttachmentDto
                    {
                        Id = insertedId,
                        FileName = file.FileName
                    });
                }

                return Json(new AjaxResponse(filesOutput));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}