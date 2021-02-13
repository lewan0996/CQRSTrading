using System;

namespace CQRSTrading.Shared.Domain.Exceptions
{
	public class DomainException : Exception, IDomainException
	{
		public DomainException(string message) : base(message) { }
	}
}
