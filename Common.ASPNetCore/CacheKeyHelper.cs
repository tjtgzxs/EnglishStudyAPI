using Microsoft.AspNetCore.Mvc;

namespace Common.ASPNetCOre;

public static class CacheKeyHelper
{
    public static string CalcCacheKeyFromAction(this ControllerBase controller)
    {
        return GetCacheKey(controller.ControllerContext);
    }
    
    public static string GetCacheKey(this ControllerContext controllerContext)
    {
        var routeValues=controllerContext.RouteData.Values.Values;
        string cacheKey=string.Join(".",routeValues);
        return cacheKey;
    }
}