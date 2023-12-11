using infrastructure.datamodels;

namespace api;

/**
 * This class provides getters and setters for the sessionData.
 */
public static class HttpContextExtensions
{
    public static void SetSessionData(this HttpContext httpContext, SessionData data)
    {
        httpContext.Session.SetInt32(SessionData.Keys.UserId, data.userid);
    }

    public static SessionData? GetSessionData(this HttpContext httpContext)
    {
        var userId = httpContext.Session.GetInt32(SessionData.Keys.UserId);
        if (userId == null) return null;
        return new SessionData()
        {
            userid = userId.Value,
        };
    }
}