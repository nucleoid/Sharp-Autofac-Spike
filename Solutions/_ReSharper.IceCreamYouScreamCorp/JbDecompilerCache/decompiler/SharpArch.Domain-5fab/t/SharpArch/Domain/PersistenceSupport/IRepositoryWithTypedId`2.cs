// Type: SharpArch.Domain.PersistenceSupport.IRepositoryWithTypedId`2
// Assembly: SharpArch.Domain, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.Domain.dll

using System.Collections.Generic;

namespace SharpArch.Domain.PersistenceSupport
{
  public interface IRepositoryWithTypedId<T, TId>
  {
    T Get(TId id);

    IList<T> GetAll();

    T SaveOrUpdate(T entity);

    void Delete(T entity);

    IList<T> PerformQuery(IQuery<T> query);
  }
}
