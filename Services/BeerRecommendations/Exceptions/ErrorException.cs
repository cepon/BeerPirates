using BeerRecommendations.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeerRecommendations.Exceptions
{
  public class ErrorException : Exception
  {
    public HttpStatusCode Status { get; private set; }
    public ErrorEnum ResponseCode { get; private set; }
    public string Descirption { get; private set; }

    public ErrorException(HttpStatusCode status, ErrorEnum responseCode, string descirption)
    {
      this.Status = status;
      this.ResponseCode = responseCode;
      this.Descirption = descirption;
    }

    // Parameter validation error.
    public ErrorException(ModelStateDictionary validationModel)
    {
      var invalidParameters = validationModel.Where(model => model.Value.ValidationState == ModelValidationState.Invalid);
      var description = "";
      foreach (var invalidParameter in invalidParameters)
      {
        if (invalidParameter.Value.Errors.Count > 0 && invalidParameter.Value.Errors[0].ErrorMessage.Contains("field is required."))
        {
          description += string.Format("Field {1} is required.", invalidParameter.Value.RawValue, invalidParameter.Key);
        }
        else
        {
          description += string.Format("Value '{0}' for {1} is not valid.", invalidParameter.Value.RawValue, invalidParameter.Key);
        }
      }

      this.Status = HttpStatusCode.BadRequest;
      this.ResponseCode = ErrorEnum.INVALID_INPUT_PARAMETERS;
      this.Descirption = description;
    }
  }
}
