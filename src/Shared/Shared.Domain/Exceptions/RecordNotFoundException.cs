using System;

namespace CQRSTrading.Shared.Domain.Exceptions
{
	public class RecordNotFoundException : Exception
	{
		public RecordNotFoundException(Uri path) : base($"Record with path {path.AbsoluteUri} was not found.") { }

		public RecordNotFoundException(Guid id) : base($"Record with id {id} was not found.") { }

		public RecordNotFoundException(string recordTypeName, Guid id) : base($"{recordTypeName} record with an id {id} was not found.") { }

		public RecordNotFoundException(string recordTypeName, string id) : base($"{recordTypeName} record with an id {id} was not found.") { }

		public RecordNotFoundException(string customMessage) : base(customMessage) { }
	}
}
