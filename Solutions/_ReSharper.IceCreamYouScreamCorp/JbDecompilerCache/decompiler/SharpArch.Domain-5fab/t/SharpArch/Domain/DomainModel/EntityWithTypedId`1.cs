// Type: SharpArch.Domain.DomainModel.EntityWithTypedId`1
// Assembly: SharpArch.Domain, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.Domain.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace SharpArch.Domain.DomainModel
{
  [Serializable]
  public abstract class EntityWithTypedId<TId> : ValidatableObject, IEntityWithTypedId<TId>
  {
    private const int HashMultiplier = 31;
    private int? cachedHashcode;

    [JsonProperty]
    [XmlIgnore]
    public virtual TId Id { get; protected set; }

    public override bool Equals(object obj)
    {
      EntityWithTypedId<TId> compareTo = obj as EntityWithTypedId<TId>;
      if (object.ReferenceEquals((object) this, (object) compareTo))
        return true;
      if (compareTo == null || !this.GetType().Equals(compareTo.GetTypeUnproxied()))
        return false;
      if (this.HasSameNonDefaultIdAs(compareTo))
        return true;
      else
        return this.IsTransient() && compareTo.IsTransient() && this.HasSameObjectSignatureAs((BaseObject) compareTo);
    }

    public override int GetHashCode()
    {
      if (this.cachedHashcode.HasValue)
        return this.cachedHashcode.Value;
      this.cachedHashcode = !this.IsTransient() ? new int?(this.GetType().GetHashCode() * 31 ^ this.Id.GetHashCode()) : new int?(base.GetHashCode());
      return this.cachedHashcode.Value;
    }

    public virtual bool IsTransient()
    {
      return (object) this.Id == null || this.Id.Equals((object) default (TId));
    }

    protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
    {
      return Enumerable.Where<PropertyInfo>((IEnumerable<PropertyInfo>) this.GetType().GetProperties(), (Func<PropertyInfo, bool>) (p => Attribute.IsDefined((MemberInfo) p, typeof (DomainSignatureAttribute), true)));
    }

    private bool HasSameNonDefaultIdAs(EntityWithTypedId<TId> compareTo)
    {
      return !this.IsTransient() && !compareTo.IsTransient() && this.Id.Equals((object) compareTo.Id);
    }
  }
}
