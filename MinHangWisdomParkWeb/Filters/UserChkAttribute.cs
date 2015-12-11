using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MinHangWisdomParkWeb.Filters
{
    public class UserChkAttribute
    {
        /// <summary>
        /// 验证用户是否登录
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class IsUserAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (GlobalParameter.UserId != null && GlobalParameter.Actorid != null)
                {
                    return;
                }
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Main", action = "Login" }));
            }
        }

    }
}