using Abp.Application.Services.Dto;

namespace PAX.TaskManager.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}