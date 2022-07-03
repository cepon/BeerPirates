using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Product.DTOs;
using Product.Exceptions;
using Product.Enums;

namespace Product.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [ApiController]
  public class ErrorController : ControllerBase
  {

    [Route("api/error")]
    public IActionResult Error()
    {
      var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
      ErrorDto error;
      HttpStatusCode status;
      if (context.Error is ErrorException errorException)
      {
        error = new ErrorDto() { ResponseCode = errorException.ResponseCode, Message = errorException.Descirption };
        status = errorException.Status;
      }
      else
      {
        error = new ErrorDto() { ResponseCode = ErrorEnum.GENERAL_ERROR, Message = "Unhandled error" };
        status = HttpStatusCode.InternalServerError;
      }

      var response = StatusCode((int)status, error);
      return response;
    }
  }
}
