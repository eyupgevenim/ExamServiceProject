using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamService
{
    //deny access signin users [SignInActionFilter]
    public class SignInActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string pathBase = context.HttpContext.Request.PathBase;
                context.HttpContext.Response.Redirect(pathBase + "/Question/Index");
            }
        }
    }
}
