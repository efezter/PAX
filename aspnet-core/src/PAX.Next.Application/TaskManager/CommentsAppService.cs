using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Authorization;
using PAX.Next.Authorization.Users;
using PAX.Next.TaskManager.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_Comments)]
    public class CommentsAppService : NextAppServiceBase, ICommentsAppService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public CommentsAppService(IRepository<Comment> commentRepository, IRepository<PaxTask, int> lookup_paxTaskRepository, IRepository<User, long> lookup_userRepository)
        {
            _commentRepository = commentRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;
            _lookup_userRepository = lookup_userRepository;

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
                               COmmentText = o.CommentText,
                               PaxTaskHeader = s1 == null || s1.Header == null ? "" : s1.Header.ToString(),
                               UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                           };

            var totalCount = await filteredComments.CountAsync();

            var dbList = await comments.ToListAsync();
            var results = new List<GetCommentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCommentForViewDto()
                {
                    Comment = new CommentDto
                    {

                        Id = o.Id,
                    },
                    PaxTaskHeader = o.PaxTaskHeader,
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

            if (output.Comment.PaxTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.Comment.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.Comment.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Comment.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCommentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Create)]
        protected virtual async Task Create(CreateOrEditCommentDto input)
        {
            input.UserId = AbpSession.GetUserId();
            var comment = ObjectMapper.Map<Comment>(input);

            await _commentRepository.InsertAsync(comment);

        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
        protected virtual async Task Update(CreateOrEditCommentDto input)
        {
            var comment = await _commentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, comment);

        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _commentRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Comments)]
        public async Task<PagedResultDto<CommentPaxTaskLookupTableDto>> GetAllPaxTaskForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_paxTaskRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Header != null && e.Header.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var paxTaskList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CommentPaxTaskLookupTableDto>();
            foreach (var paxTask in paxTaskList)
            {
                lookupTableDtoList.Add(new CommentPaxTaskLookupTableDto
                {
                    Id = paxTask.Id,
                    DisplayName = paxTask.Header?.ToString()
                });
            }

            return new PagedResultDto<CommentPaxTaskLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Comments)]
        public async Task<PagedResultDto<CommentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CommentUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new CommentUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<CommentUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}