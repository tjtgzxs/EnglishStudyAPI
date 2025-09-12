using Microsoft.AspNetCore.Mvc;

namespace Common.ASPNetCOre;

public static class ControllerBaseExtensions
{
    public static BadRequestObjectResult ApiError(this ControllerBase controller, int code, string message)
    {
        return controller.BadRequest(new ApiErrors(code, message));
    }
}