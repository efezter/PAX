using System;

namespace PAX.Next.Authentication.TwoFactor
{
    [Serializable]
    public class TwoFactorCodeCacheItem
    {
        public const string CacheName = "AppTwoFactorCodeCache";

        public string Code { get; set; }

        public static readonly TimeSpan DefaultSlidingExpireTime = TimeSpan.FromMinutes(2);

        public TwoFactorCodeCacheItem()
        {
            
        }

        public TwoFactorCodeCacheItem(string code)
        {
            Code = code;
        }
    }
}
