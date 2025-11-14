using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class LoginFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var usuarioId = session.GetInt32("UsuarioId");

        if (usuarioId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }

        base.OnActionExecuting(context);
    }
}
