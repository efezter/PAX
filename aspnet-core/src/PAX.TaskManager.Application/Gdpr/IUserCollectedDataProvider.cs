using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using PAX.TaskManager.Dto;

namespace PAX.TaskManager.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
