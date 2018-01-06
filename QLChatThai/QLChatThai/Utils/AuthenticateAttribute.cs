using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Utils
{
    public class AuthenticateAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!XUser.IsAuthenticated)
            {
                HttpContext.Current.Session["ReturnUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;
                HttpContext.Current.Response.Redirect("/Account/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}