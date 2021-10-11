using System;
using System.Threading.Tasks;
using PAX.TaskManager.ApiClient.Models;

namespace PAX.TaskManager.ApiClient
{
    public interface IAccessTokenManager
    {
        string GetAccessToken();

        Task<AbpAuthenticateResultModel> LoginAsync();

        Task<string> RefreshTokenAsync();

        void Logout();

        bool IsUserLoggedIn { get; }

        bool IsRefreshTokenExpired { get; }

        AbpAuthenticateResultModel AuthenticateResult { get; set; }

        DateTime AccessTokenRetrieveTime { get; set; }
    }
}