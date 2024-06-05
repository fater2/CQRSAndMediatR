﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CQRSAndMediatR.Exceptions.AnotherExceptionsHandler
{
	public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		: IExceptionHandler
	{
		// this method from IExceptionHandler
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			ProblemDetails problemDetails = new()
			{
				Instance = httpContext.Request.Path
			};
			if (exception is BaseException e)
			{
				httpContext.Response.StatusCode = (int)e.StatusCode;
				problemDetails.Title = e.Message;
			}
			else
			{
				problemDetails.Title = exception.Message;
			}
			logger.LogError("****{ProblemDetailsTitle}", problemDetails.Title);

			problemDetails.Status = httpContext.Response.StatusCode;

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
			
			return true;//true if the exception is handled as required.

		}
	}
}
