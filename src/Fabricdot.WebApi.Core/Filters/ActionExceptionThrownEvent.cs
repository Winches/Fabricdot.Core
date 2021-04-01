using System;
using Fabricdot.Core.ExceptionHandling;
using MediatR;

namespace Fabricdot.WebApi.Core.Filters
{
    public class ActionExceptionThrownEvent : ExceptionThrownEvent, INotification
    {
        public ActionExceptionThrownEvent(Exception exception) : base(exception, null)
        {
        }
    }
}