using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebProject.Filters
{
    public class HotelNotFoundExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            context.Result = new ContentResult
            {
                Content = $"In method {actionName} an exception occurred: \n {exceptionMessage} \n {exceptionStack}",
                StatusCode = (int?)HttpStatusCode.NotFound
            };

            context.ExceptionHandled = true;


        }
    }
}
