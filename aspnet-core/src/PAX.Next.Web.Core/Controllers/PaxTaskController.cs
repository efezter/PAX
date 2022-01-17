using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PAX.Next.Authorization.Users;
using PAX.Next.TaskManager;
using PAX.Next.TaskManager.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PAX.Next.Web.Controllers
{
    [AbpMvcAuthorize]
    public class PaxTaskController : NextControllerBase
    {
        private readonly PaxTaskAttachmentsAppService _paxTaskAttachmentsAppService;
        private readonly IHostEnvironment _env;
        private readonly IRepository<User, long> _lookup_userRepository;


        public PaxTaskController(PaxTaskAttachmentsAppService paxTaskAttachmentsAppService, IHostEnvironment env, IRepository<User, long> lookup_userRepository)
        {
            _env = env;
            _paxTaskAttachmentsAppService = paxTaskAttachmentsAppService;
            _lookup_userRepository = lookup_userRepository;
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles([FromQuery(Name = "taskId")] int taskId)
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
                GetAllPaxTaskAttachmentsInput getAtchInput = new GetAllPaxTaskAttachmentsInput();
                int lastInx;
                string fileName = string.Empty;

                foreach (var file in files)
                {
                    if (file.Length > 1048576) //1MB
                    {
                        throw new UserFriendlyException(L("File_SizeLimit_Error"));
                    }


                    fileName = file.FileName;
                    getAtchInput.TaskId = taskId;
                    getAtchInput.Filter = file.FileName;

                    var atch = await _paxTaskAttachmentsAppService.GetAll(getAtchInput);

                    if (atch.TotalCount > 0)
                    {
                        lastInx = fileName.LastIndexOf('.');
                        fileName = fileName.Substring(0, lastInx) + "(" + (atch.TotalCount + 1).ToString() + ")" + Path.GetExtension(fileName);
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
                    var filePath = Path.Combine(dir, fileName);

                    System.IO.File.WriteAllBytes(filePath, fileBytes);


                   


                    CreateOrEditPaxTaskAttachmentDto attchDto = new CreateOrEditPaxTaskAttachmentDto();
                    attchDto.PaxTaskId = taskId;
                    attchDto.FileName = fileName;

                    int insertedId = await _paxTaskAttachmentsAppService.CreateOrEdit(attchDto);

                    filesOutput.Add(new PaxTaskAttachmentDto
                    {
                        Id = insertedId,
                        FileName = fileName,
                        FileUrl = AbpSession.TenantId + "/Tasks/" + taskId + "/Attachments/" + fileName,
                        CreationTime = DateTime.Now,
                        UserName = _lookup_userRepository.Get(AbpSession.UserId.Value).FullName
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