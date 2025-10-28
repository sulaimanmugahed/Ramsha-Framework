using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Ramsha.UnitOfWork;

namespace Ramsha.AspNetCore;

public class UnitOfWorkMiddleware(RequestDelegate next, IUnitOfWorkManager unitOfWorkManager)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using (var uow = unitOfWorkManager.Reserve(UnitOfWork.UnitOfWork.UnitOfWorkReservationName))
        {
            await next(context);
            await uow.CompleteAsync();
        }
    }
}

public static class UnitOfWorkMiddlewareExtensions
{
    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UnitOfWorkMiddleware>();
    }
}