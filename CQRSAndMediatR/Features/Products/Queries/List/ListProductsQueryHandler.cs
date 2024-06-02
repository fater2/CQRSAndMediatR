using CQRSAndMediatR.Features.Products.Dtos;
using CQRSAndMediatR.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSAndMediatR.Features.Products.Queries.List
{
	// IRequestHandler<T,R>:
	// T is the incoming request
	// R is the response
	public class ListProductsQueryHandler(AppDbContext dbContext) : IRequestHandler<ListProductQuery, List<ProductDto>>
	{
		public async Task<List<ProductDto>> Handle(ListProductQuery request, CancellationToken cancellationToken)
		{
			return await dbContext.Products
		   .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price))
		   .ToListAsync();
		}
	}
}
