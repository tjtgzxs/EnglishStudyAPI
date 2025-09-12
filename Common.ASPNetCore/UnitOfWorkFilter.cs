using System.Reflection;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ASPNetCOre;

public class UnitOfWorkFilter:IAsyncActionFilter
{
    private static UnitOfWorkAttribute? GetUoWAttribute(ActionDescriptor actionDescriptor)
    {
        var caDesc = actionDescriptor as ControllerActionDescriptor;
        if (caDesc == null)
        {
             return null;   
        }

        var uawAttr = caDesc.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        if (uawAttr != null)
        {
            return uawAttr;
        }
        else
        {
            return caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            
        }
    }
    
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var uowAttr=GetUoWAttribute(context.ActionDescriptor);
        if (uowAttr == null)
        {
            await next();
            return;
        }
        using TransactionScope txScope=new (TransactionScopeAsyncFlowOption.Enabled);
        List<DbContext> dbCtxs=new List<DbContext>();
        foreach (var dbContextType in uowAttr.DbContextTypes)
        {
            var sp=context.HttpContext.RequestServices;
            DbContext? dbCtx = (DbContext)sp.GetRequiredService(dbContextType);
            dbCtxs.Add(dbCtx);
            
        }
        var result=await next();
        if (result.Exception == null)
        {
            foreach (var dbContext in dbCtxs)
            {
                await dbContext.SaveChangesAsync();
            }
            txScope.Complete();
        }
    }
}