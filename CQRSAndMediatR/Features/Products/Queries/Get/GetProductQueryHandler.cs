using CQRSAndMediatR.Features.Products.Dtos;
using CQRSAndMediatR.Persistence;
using MediatR;

namespace CQRSAndMediatR.Features.Products.Queries.Get
{
	public class GetProductQueryHandler(AppDbContext dbContext)
		: IRequestHandler<GetProductQuery, ProductDto?>
	{
		public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var product = await dbContext.Products.FindAsync(request.Id);
			if (product == null)
			{
				return null;
			}
			return new ProductDto(product.Id, product.Name, product.Description, product.Price);
		}
	}
}
