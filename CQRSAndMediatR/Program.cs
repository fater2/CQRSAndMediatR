using CQRSAndMediatR.Exceptions;
using CQRSAndMediatR.Exceptions.AnotherExceptionsHandler;
using CQRSAndMediatR.Features.Products.Commands.Create;
using CQRSAndMediatR.Features.Products.Commands.Delete;
using CQRSAndMediatR.Features.Products.Queries.Get;
using CQRSAndMediatR.Features.Products.Queries.List;
using CQRSAndMediatR.Notifications;
using CQRSAndMediatR.Persistence;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register the dbcontect
builder.Services.AddDbContext<AppDbContext>();

// Registering MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// 2.1/2.2 global exception handler 
builder.Services.AddExceptionHandler<ProductNotFoundExceptionHandler>();// this will come first
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();// this will come second
builder.Services.AddProblemDetails();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// 1 global exception handler 
// add global exceptions handler middleware
// note to comment UseExceptionHandler
//app.UseMiddleware<ErrorHandlerMiddleware>();

// 2.1/2.2 global exception handler 
app.UseExceptionHandler();

#region Minimal API Endpoints
// ISender is to send the commands/queries to its registered handlers.
app.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
	var product = await mediatr.Send(new GetProductQuery(id));
	return product == null 
		? throw new ProductNotFoundException(id) 
		: Results.Ok(product);
});

app.MapGet("/products", async (ISender mediatr) => // or use IMediator instead of ISender, but the ISender interface is far more lightweight
{
	var products = await mediatr.Send(new ListProductQuery());
	return Results.Ok(products);
});

app.MapPost("/products", async (CreateProductCommand command, IMediator mediatr) =>
{
	var productId = await mediatr.Send(command);
	if (Guid.Empty == productId) return Results.BadRequest();
	await mediatr.Publish(new ProductCreatedNotification(productId));
	return Results.Created($"/products/{productId}", new { id = productId });
});

app.MapDelete("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
	await mediatr.Send(new DeleteProductCommand(id));
	return Results.NoContent();
});


#endregion

app.Run();
