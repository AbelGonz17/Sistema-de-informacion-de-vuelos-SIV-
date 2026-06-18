using Microsoft.AspNetCore.Mvc;
using SIV.Domain.Common;

namespace SIV.Presentation.Common
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result)
        {
            if (result.IsSuccess)
                return new OkResult();

            return new ObjectResult(new { error = result.ErrorMessage })
            {
                StatusCode = result.StatusCode
            };
        }

        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (typeof(T) == typeof(string))
                {
                    return new OkObjectResult(new { tokenAccess = result.Value, tipo = "Bearer" });
                }

                return new OkObjectResult(result.Value);
            }

            return new ObjectResult(new { error = result.ErrorMessage })
            {
                StatusCode = result.StatusCode
            };
        }
    }
}