using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Stump.Server.WorldServer.WebAPI
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            IEnumerable<string> headerValues;
            var apiKey = string.Empty;
            var keyFound = actionContext.Request.Headers.TryGetValues("APIKey", out headerValues);
            if (keyFound)
            {
                apiKey = headerValues.FirstOrDefault();
            }

            return apiKey == WebServer.WebAPIKey;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                return;
            }

            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}