using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.filters;

/**
 * This class can be implemented with the "[RequireAuthentication]", to protect the controller from onAuthenticated users
 */
public class RequireAuthentication : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.GetSessionData() == null) throw new AuthenticationException();
    }
}