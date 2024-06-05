using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace CQRSAndMediatR.Exceptions.AnotherExceptionsHandler
{
	public class ProductNotFoundExceptionHandler(ILogger<ProductNotFoundExceptionHandler> logger) : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			if (exception is not ProductNotFoundException e)
			{
				return false;
			}

			//handle error
			ProblemDetails problemDetails = new()
			{
				Instance = httpContext.Request.Path,
				Title = e.Message,
				Status = httpContext.Response.StatusCode
			};
			httpContext.Response.StatusCode = (int)e.StatusCode;
			logger.LogError("****{ProblemDetailsTitle}", problemDetails.Title);
			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);

			return true;
		}
	}
}
