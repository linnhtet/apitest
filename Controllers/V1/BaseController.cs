using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using WebApi.Helpers;
using WebApi.Models.Token;

namespace WebApi.Controllers.V1
{
    [ApiController]
    public class BaseController : Controller
    {
        public TokenInfo TokenInfo { get; private set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            StringValues tokenString = base.Request.Headers["Authorization"];
            var appsetting = context.HttpContext.RequestServices.GetRequiredService<IOptions<AppSettings>>();
            var token = tokenString.ToString().Replace("Bearer ", "");

            var info = TokenHelper.ReteiveTokenInfo(token, appsetting.Value.Secret);
            if (info == null)
            {
                throw new UnauthorizedAccessException();
            }

            TokenInfo = info;
        }
    }
}
