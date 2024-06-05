using INotification=MediatR.INotification;

namespace CQRSAndMediatR.Notifications
{
	 public record ProductCreatedNotification(Guid Id):INotification;

}
