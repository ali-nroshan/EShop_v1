using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebApp.Filters
{

    public static class UsersListCheckerTool
    {
        private static List<int> _usersId = new List<int>();

        public static void AddUserToRemoveList(int userId)
        {
            _usersId.Add(userId);
        }

        public static bool IsUserInList(int userId)
            => _usersId.Contains(userId);

        public static void RemoveUserFromList(int userId)
        {
            if (_usersId.Contains(userId))
                _usersId.Remove(userId);
        }
    }
    public sealed class CheckUserRoleFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool user_auth_state =
                context.HttpContext.User.Identity!.IsAuthenticated;
            bool isAdmin = false;

            if (user_auth_state)
                isAdmin = bool.Parse(context.HttpContext.User.FindFirstValue("IsAdmin")!);


            if (context.HttpContext.Request.Path.StartsWithSegments("/Admin"))
            {
                if (!user_auth_state || !isAdmin)
                {
                    context.Result = new RedirectResult("/404");
                    return;
                }
            }

            if (user_auth_state)
            {
                int userId = int
                    .Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!
                    .ToString());
                if (UsersListCheckerTool.IsUserInList(userId))
                {
                    context.Result = new RedirectResult("/Logout");
                    UsersListCheckerTool.RemoveUserFromList(userId);
                }
            }
        }
    }
}
