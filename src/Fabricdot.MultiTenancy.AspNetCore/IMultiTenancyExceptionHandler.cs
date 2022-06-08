using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.MultiTenancy.AspNetCore;

public interface IMultiTenancyExceptionHandler
{
    Task HandleAsync(HttpContext context, Exception exception);
}