using infrastructure.datamodels;

namespace api;

/**
 * This class provides getters and setters for the sessionData.
 */
public static class HttpContextExtensions
{
        public static void SetSessionData(this HttpContext httpContext, SessionData data)
        {
            httpContext.Items["data"] = data;
        }

        public static SessionData? GetSessionData(this HttpContext httpContext)
        {
            return httpContext.Items["data"] as SessionData;
        }
}