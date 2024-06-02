using CQRSAndMediatR.Domain;
using CQRSAndMediatR.Persistence;
using MediatR;

namespace CQRSAndMediatR.Features.Products.Commands.Create
{
	public class CreateProductCommandHandler(AppDbContext dbContext)
		: IRequestHandler<CreateProductCommand, Guid>
	{
		public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
		{
			var product = new Product(command.Name, command.Description, command.Price);
			await dbContext.Products.AddAsync(product);
			await dbContext.SaveChangesAsync();
			return product.Id;
		}
	}
}
