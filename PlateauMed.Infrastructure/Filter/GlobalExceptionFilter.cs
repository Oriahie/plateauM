using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Models;
using PlateauMed.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var (responseModel, statusCode) = GetStatusCode<object>(context.Exception);
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            context.Result = new JsonResult(responseModel);
        }

        public static (ResponseModel<T> responseModel, HttpStatusCode statusCode) GetStatusCode<T>(Exception exception)
        {

            return exception switch
            {
                BaseException bex => (new ResponseModel<T>
                {
                    Message = bex.Message,
                    Status = false
                }, bex.HttpStatusCode),
                _ => (new ResponseModel<T>
                {
                    Message = "An Error Occured, Please Try Again",
                    Status = false
                }, HttpStatusCode.InternalServerError)
            };
        }

    }
}
