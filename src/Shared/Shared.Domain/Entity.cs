﻿using System;
using System.Collections.Generic;
using MediatR;

namespace CQRSTrading.Shared.Domain
{
	public abstract class Entity
	{
		private int _hashCode;

		private int HashCode
		{
			get
			{
				if (IsTransient())
				{
					return base.GetHashCode();
				}

				if (_hashCode == default)
				{
					_hashCode = Id.GetHashCode() ^ 31;
				}

				return _hashCode;
			}
		}

		public Guid Id { get; }

		private List<INotification> _domainEvents;
		public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

		protected Entity()
		{
			Id = Guid.NewGuid();
		}

		protected Entity(Guid id)
		{
			Id = id;
		}

		public void AddDomainEvent(INotification eventItem)
		{
			_domainEvents ??= new List<INotification>();
			_domainEvents.Add(eventItem);
		}

		public void RemoveDomainEvent(INotification eventItem)
		{
			_domainEvents?.Remove(eventItem);
		}

		public void ClearDomainEvents()
		{
			_domainEvents?.Clear();
		}

		public bool IsTransient() => Id == default;

		public override bool Equals(object obj)
		{
			if (!(obj is Entity))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (GetType() != obj.GetType())
			{
				return false;
			}

			var item = (Entity)obj;

			if (item.IsTransient() || IsTransient())
			{
				return false;
			}

			return item.Id == Id;
		}

		public override int GetHashCode() => HashCode;

		public static bool operator ==(Entity left, Entity right) => left?.Equals(right) ?? Equals(right, null);

		public static bool operator !=(Entity left, Entity right) => !(left == right);
	}
}
