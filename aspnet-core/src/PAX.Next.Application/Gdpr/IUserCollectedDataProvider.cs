using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using PAX.Next.Dto;

namespace PAX.Next.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
