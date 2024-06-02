using CQRSAndMediatR.Features.Products.Dtos;
using MediatR;

namespace CQRSAndMediatR.Features.Products.Queries.List
{
	// IRequest<T> interface of the MediatR library
	public record ListProductQuery: IRequest<List<ProductDto>>;

}
