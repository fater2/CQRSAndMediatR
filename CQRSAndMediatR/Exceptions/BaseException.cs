using System.Net;

namespace CQRSAndMediatR.Exceptions;

public class BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
	: Exception(message)
{
	public HttpStatusCode StatusCode { get; } = statusCode;
}
