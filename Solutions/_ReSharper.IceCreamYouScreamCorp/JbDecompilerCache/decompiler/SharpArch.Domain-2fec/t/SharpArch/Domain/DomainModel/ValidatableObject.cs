// Type: SharpArch.Domain.DomainModel.ValidatableObject
// Assembly: SharpArch.Domain, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\Solutions\IceCreamYouScreamCorp.Web.Mvc\Bin\SharpArch.Domain.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharpArch.Domain.DomainModel
{
  [Serializable]
  public abstract class ValidatableObject : BaseObject
  {
    public virtual bool IsValid()
    {
      return this.ValidationResults().Count == 0;
    }

    public virtual ICollection<ValidationResult> ValidationResults()
    {
      List<ValidationResult> list = new List<ValidationResult>();
      Validator.TryValidateObject((object) this, new ValidationContext((object) this, (IServiceProvider) null, (IDictionary<object, object>) null), (ICollection<ValidationResult>) list, true);
      return (ICollection<ValidationResult>) list;
    }
  }
}
