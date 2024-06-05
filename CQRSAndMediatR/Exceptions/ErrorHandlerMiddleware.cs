using CQRSAndMediatR.Features.Products.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CQRSAndMediatR.Exceptions
{
	public class ErrorHandlerMiddleware(RequestDelegate _next, ILogger<ErrorHandlerMiddleware> logger)
	{
		public async Task Invoke(HttpContext context)
		{
			try
			{
				// this is allways will be executed 
				// _next(context) make the next statement the body of your http request "get, post, etc."
				await _next(context);
			}
			catch(Exception error)
			{
				// this will be executed only in case of exceptions
				var response = context.Response;
				response.ContentType = "application/json";
				response.StatusCode = error switch
				{
					BaseException e => (int)e.StatusCode,
					_ => StatusCodes.Status500InternalServerError,
				};
				//problemDetail could be of any other type 
				ProblemDetails problemDetail = new()
				{
					Status = response.StatusCode,
					Title = error.Message
				};
				logger.LogError(message: $"ErrorHandlerMiddleware, Exception catched \n{error.Message}");
				string result = JsonSerializer.Serialize( problemDetail );
				await response.WriteAsync( result );
			}
		}
	}
}
