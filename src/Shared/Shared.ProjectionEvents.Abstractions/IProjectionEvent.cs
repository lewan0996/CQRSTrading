namespace CQRSTrading.Shared.ProjectionEvents.Abstractions
{
	public interface IProjectionEvent
	{
		string EventType { get; }
	}
}