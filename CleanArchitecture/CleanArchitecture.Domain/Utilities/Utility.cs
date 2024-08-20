using CleanArchitecture.Domain.Extensions;
using CleanArchitecture.Domain.Utilities.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Utilities
{
    public abstract class Utility
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static ICacheService _memoryCache;
        public static dynamic _menuPermissionManager;

        public static void Configure(IHttpContextAccessor httpContextAccessor, ICacheService memoryCache, dynamic menuPermissionManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _menuPermissionManager = menuPermissionManager;
        }

        public static Nullable<Guid> UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (id != null)
                {
                    Guid userId = Guid.Empty;
                    Guid.TryParse(id.Value, out userId);
                    return userId;
                }
                return null;
            }
        }

        public static string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext.User.Identity.Name ?? String.Empty;
            }
        }

        public static DateTime? GetTokenExpireTime
        {
            get
            {
                var exp = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "exp").Value;
                if (exp != null)
                {
                    if (long.TryParse(exp, out long expValue))
                    {
                        return expValue.UnixTimeStampToDateTime();
                    }
                }
                return null;
            }
        }

    }
}
