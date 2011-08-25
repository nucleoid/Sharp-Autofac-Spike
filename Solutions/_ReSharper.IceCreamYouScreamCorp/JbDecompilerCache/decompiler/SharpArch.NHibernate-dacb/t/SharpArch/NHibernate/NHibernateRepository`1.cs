// Type: SharpArch.NHibernate.NHibernateRepository`1
// Assembly: SharpArch.NHibernate, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.NHibernate.dll

using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate.Contracts.Repositories;

namespace SharpArch.NHibernate
{
  public class NHibernateRepository<T> : NHibernateRepositoryWithTypedId<T, int>, INHibernateRepository<T>, INHibernateRepositoryWithTypedId<T, int>, IRepository<T>, IRepositoryWithTypedId<T, int>
  {
  }
}
