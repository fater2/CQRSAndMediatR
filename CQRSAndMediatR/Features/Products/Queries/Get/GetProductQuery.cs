using CQRSAndMediatR.Features.Products.Dtos;
using MediatR;

namespace CQRSAndMediatR.Features.Products.Queries.Get
{
	public record GetProductQuery(Guid Id) : IRequest<ProductDto>;

}
