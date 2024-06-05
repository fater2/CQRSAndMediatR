using System.Net;

namespace CQRSAndMediatR.Exceptions;

public class ProductNotFoundException(Guid id) 
	: BaseException($"product with id {id} not found", HttpStatusCode.NotFound)
{
}
