using System;

namespace CQRSTrading.Shared.ReadModel
{
	public abstract class ReadModelEntity
	{
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public Guid Id { get; set; }
	}
}
