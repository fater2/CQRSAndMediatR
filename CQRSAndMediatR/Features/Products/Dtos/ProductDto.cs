namespace CQRSAndMediatR.Features.Products.Dtos
{
	//Quick Tip: It’s recommended to use records to define Dtos, as they are:
	//	immutable by default!
	//	used only to transfere state and nothing else
	//  one-way flow

	public record ProductDto(Guid Id, string Name, string Description, decimal Price);


}
