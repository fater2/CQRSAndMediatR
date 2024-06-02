using MediatR;

namespace CQRSAndMediatR.Features.Products.Commands.Delete;

public record DeleteProductCommand(Guid Id) : IRequest;// void response
