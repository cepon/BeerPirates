using BeerRecommendations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeerRecommendations.DTOs
{
  public class ErrorDto
  {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ErrorEnum ResponseCode { get; set; }
    public string Message { get; set; }
  }
}
