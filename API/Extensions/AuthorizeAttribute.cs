using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System;
using GeekOff.Models;
using GeekOff.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles;

    public AuthorizeAttribute(params Role[] roles)
    {
        _roles = roles ?? Array.Empty<Role>();
    }


    // this works when not logged in
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var notAuth = false;
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        // a very poor implementation of early exit
        var teamNum = context.HttpContext.Items["User"];
        if (teamNum is null)
        {
            notAuth = true;
        }
        var teamCheck = int.TryParse(teamNum.ToString(), out var teamNumInt);
        if (!teamCheck || teamNumInt < 0)
        {
            notAuth = true;
        }

        var adminName = context.HttpContext.Items["Name"];
        if (teamNumInt > 0 && adminName is null)
        {
            notAuth = false;
        }

        if (teamNumInt == 0 && adminName is null)
        {
            notAuth = true;
        }

        var roleTest = Enum.TryParse<Role>(context.HttpContext.Items["Role"].ToString(), out var roleName);
        if (roleTest == false)
        {
            notAuth = true;
        }

        if (_roles.Any() && _roles.Contains(roleName) && notAuth != true)
        {
            notAuth = false;
        }
        else
        {
            notAuth = true;
        }

        if (notAuth)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
