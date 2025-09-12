﻿using Microsoft.EntityFrameworkCore;

namespace Common.ASPNetCOre;

[AttributeUsage(AttributeTargets.Method|AttributeTargets.Class,AllowMultiple=false,Inherited=true)]
public class UnitOfWorkAttribute: Attribute
{
    public Type[] DbContextTypes { get;  init;  }

    public UnitOfWorkAttribute(params Type[] dbContextTypes)
    {
        this.DbContextTypes = dbContextTypes;
        foreach (var type in dbContextTypes)
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
            {
                throw new ArgumentException("${type} must inherit from DbContext");
            }
        }
    }
    
}