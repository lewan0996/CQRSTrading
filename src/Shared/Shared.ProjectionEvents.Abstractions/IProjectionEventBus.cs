using System.Threading.Tasks;

namespace CQRSTrading.Shared.ProjectionEvents.Abstractions
{
	public interface IProjectionEventBus
	{
		Task PublishAsync(IProjectionEvent projectionEvent);
	}
}
