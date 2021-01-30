using CQRSTrading.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRSTrading.Shared.Infrastructure
{
	public class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
	{
		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasKey(e => e.Id);
		}
	}
}
