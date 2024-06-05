using MediatR;

namespace CQRSAndMediatR.Notifications;


public class StockAssignedHandler(ILogger<StockAssignedHandler> logger) : INotificationHandler<ProductCreatedNotification>
{
	public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
	{
		logger.LogInformation($"---1 handling notification for product creation with id : {notification.Id}. assigning stocks.");
		return Task.CompletedTask;
	}
}