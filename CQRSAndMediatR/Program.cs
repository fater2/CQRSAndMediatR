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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

#region Minimal API Endpoints
// ISender is to send the commands/queries to its registered handlers.
app.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
	var product = await mediatr.Send(new GetProductQuery(id));
	if (product == null) return Results.NotFound();
	return Results.Ok(product);
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
