using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var nivel = session.GetInt32("UsuarioNivelId");

        if (nivel != 1)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }

        base.OnActionExecuting(context);
    }
}
