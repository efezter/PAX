using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Events.Bus.Entities;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Authorization;
using PAX.Next.Authorization.Users;
using PAX.Next.TaskManager.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PAX.Next.Notifications;
using Abp;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_Comments)]
    public class CommentsAppService : NextAppServiceBase, ICommentsAppService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly ITaskHistoriesAppService _taskHistoriesAppService;
        private readonly IAppNotifier _appNotifier;

        public CommentsAppService(IRepository<Comment> commentRepository, IRepository<PaxTask, int> lookup_paxTaskRepository, IRepository<User, long> lookup_userRepository, ITaskHistoriesAppService taskHistoriesAppService, IAppNotifier appNotifier)
        {
            _commentRepository = commentRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;
            _lookup_userRepository = lookup_userRepository;
            _taskHistoriesAppService = taskHistoriesAppService;
            _appNotifier = appNotifier;
            
        }

        public async Task<PagedResultDto<GetCommentForViewDto>> GetAll(GetAllCommentsInput input)
        {

            var filteredComments = _commentRepository.GetAll()
                        //.Include(e => e.PaxTaskFk)
                        .Include(e => e.UserFk)
                        .WhereIf(input.TaskIdFilter != 0, e => e.PaxTaskFk.Id == input.TaskIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CommentText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaxTaskHeaderFilter), e => e.PaxTaskFk != null && e.PaxTaskFk.Header == input.PaxTaskHeaderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredComments = filteredComments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var comments = from o in pagedAndFilteredComments
                           join o1 in _lookup_paxTaskRepository.GetAll() on o.PaxTaskId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new
                           {
                               Id = o.Id,
                               CreatorUserId = o.CreatorUserId,
                               CreationTime = o.CreationTime,
                               LastModificationTime = o.LastModificationTime,
                               CommentText = o.CommentText,
                               UserName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                           };

            var totalCount = await filteredComments.CountAsync();

            var dbList = await comments.OrderByDescending(e => e.CreationTime).ToListAsync();
            var results = new List<GetCommentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCommentForViewDto()
                {
                    Comment = new CommentDto
                    {
                        Id = o.Id,
                        CommentText = o.CommentText,
                        UserId = o.CreatorUserId.Value
                    },
                    LastModificationTime = o.LastModificationTime,
                    CreationTime = o.CreationTime,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCommentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
        public async Task<GetCommentForEditOutput> GetCommentForEdit(EntityDto input)
        {
            var comment = await _commentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCommentForEditOutput { Comment = ObjectMapper.Map<CreateOrEditCommentDto>(comment) };

            if (output.Comment.PaxTaskId != 0)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.Comment.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.Comment.UserId != 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Comment.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [UseCase(Description = "Assign an issue to a user")]
        public async Task<CommentDto> CreateOrEdit(CreateOrEditCommentDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Create)]
        protected virtual async Task<CommentDto> Create(CreateOrEditCommentDto input)
        {
            input.UserId = AbpSession.GetUserId();
            var comment = ObjectMapper.Map<Comment>(input);

            await _commentRepository.InsertAsync(comment);

            CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = input.PaxTaskId, FieldName = "Comments", CreatedUser = input.UserId, ChangeType = EntityChangeType.Created, CreatedDate = DateTime.Now };
            await _taskHistoriesAppService.CreateOrEdit(historyDto);

            await CurrentUnitOfWork.SaveChangesAsync();

            CheckForComment(input);

            return ObjectMapper.Map<CommentDto>(comment);
        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
        protected virtual async Task<CommentDto> Update(CreateOrEditCommentDto input)
        {
            input.UserId = AbpSession.GetUserId();
            var comment = await _commentRepository.FirstOrDefaultAsync((int)input.Id);
            //ObjectMapper.Map(input, comment);

            comment.CommentText = input.CommentText;

            CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = input.PaxTaskId, FieldName = "Comments", CreatedUser = input.UserId, ChangeType = EntityChangeType.Updated, CreatedDate = DateTime.Now };
            await _taskHistoriesAppService.CreateOrEdit(historyDto);

            CheckForComment(input);

            return ObjectMapper.Map<CommentDto>(comment);
        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Delete)]
        public async Task Delete(EntityDto input)
        {
            var comment = _commentRepository.Get(input.Id);

            CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = comment.PaxTaskId, FieldName = "Comments", CreatedUser = AbpSession.GetUserId(), ChangeType = EntityChangeType.Deleted, CreatedDate = DateTime.Now };
            await _taskHistoriesAppService.CreateOrEdit(historyDto);

            await _commentRepository.DeleteAsync(input.Id);
        }

        private void CheckForComment(CreateOrEditCommentDto input)
        {
            if (input.CommentText.Contains("data-mention")) ;
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(input.CommentText);
                var mention = doc.DocumentNode.SelectSingleNode("//a[@data-user-id]");

                if (mention != null)
                {
                    var userId = int.Parse(mention.Attributes["data-user-id"].Value);

                    string changerUserName = _lookup_userRepository.Get(AbpSession.UserId.Value).FullName;

                    List<UserIdentifier> notificators = new List<UserIdentifier> { new UserIdentifier(AbpSession.TenantId, userId)};

                    _appNotifier.TaskChangedAsync(changerUserName, input.PaxTaskId, "CommentNotification", notificators.ToArray());
                }
            }
        }
    }
}