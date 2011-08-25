// Type: SharpArch.NHibernate.NHibernateRepositoryWithTypedId`2
// Assembly: SharpArch.NHibernate, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.NHibernate.dll

using NHibernate;
using NHibernate.Criterion;
using SharpArch.Domain;
using SharpArch.Domain.DomainModel;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate.Contracts.Repositories;
using System.Collections.Generic;
using System.Reflection;

namespace SharpArch.NHibernate
{
  public class NHibernateRepositoryWithTypedId<T, TId> : INHibernateRepositoryWithTypedId<T, TId>, IRepositoryWithTypedId<T, TId>
  {
    private IDbContext dbContext;

    public virtual IDbContext DbContext
    {
      get
      {
        if (this.dbContext == null)
          this.dbContext = (IDbContext) new DbContext(SessionFactoryKeyHelper.GetKeyFrom((object) this));
        return this.dbContext;
      }
    }

    protected virtual ISession Session
    {
      get
      {
        return NHibernateSession.CurrentFor(SessionFactoryKeyHelper.GetKeyFrom((object) this));
      }
    }

    IDbContext INHibernateRepositoryWithTypedId<T, TId>.DbContext
    {
      get
      {
        return this.DbContext;
      }
    }

    public virtual void Evict(T entity)
    {
      this.Session.Evict((object) entity);
    }

    public virtual IList<T> FindAll(T exampleInstance, params string[] propertiesToExclude)
    {
      ICriteria criteria = this.Session.CreateCriteria(typeof (T));
      Example example = Example.Create((object) exampleInstance);
      foreach (string str in propertiesToExclude)
        example.ExcludeProperty(str);
      criteria.Add((ICriterion) example);
      return (IList<T>) criteria.List<T>();
    }

    public virtual IList<T> FindAll(IDictionary<string, object> propertyValuePairs)
    {
      // ISSUE: unable to decompile the method.
    }

    public virtual T FindOne(T exampleInstance, params string[] propertiesToExclude)
    {
      IList<T> all = this.FindAll(exampleInstance, propertiesToExclude);
      if (all.Count > 1)
        throw new NonUniqueResultException(all.Count);
      if (all.Count == 1)
        return all[0];
      else
        return default (T);
    }

    public virtual T FindOne(IDictionary<string, object> propertyValuePairs)
    {
      IList<T> all = this.FindAll(propertyValuePairs);
      if (all.Count > 1)
        throw new NonUniqueResultException(all.Count);
      if (all.Count == 1)
        return all[0];
      else
        return default (T);
    }

    public virtual T Get(TId id, Enums.LockMode lockMode)
    {
      return (T) this.Session.Get<T>((object) id, NHibernateRepositoryWithTypedId<T, TId>.ConvertFrom(lockMode));
    }

    public virtual T Load(TId id)
    {
      return (T) this.Session.Load<T>((object) id);
    }

    public virtual T Load(TId id, Enums.LockMode lockMode)
    {
      return (T) this.Session.Load<T>((object) id, NHibernateRepositoryWithTypedId<T, TId>.ConvertFrom(lockMode));
    }

    public virtual T Save(T entity)
    {
      this.Session.Save((object) entity);
      return entity;
    }

    public virtual T Update(T entity)
    {
      this.Session.Update((object) entity);
      return entity;
    }

    public virtual void Delete(T entity)
    {
      this.Session.Delete((object) entity);
    }

    public virtual T Get(TId id)
    {
      return (T) this.Session.Get<T>((object) id);
    }

    public virtual IList<T> GetAll()
    {
      return (IList<T>) this.Session.CreateCriteria(typeof (T)).List<T>();
    }

    public IList<T> PerformQuery(IQuery<T> query)
    {
      return query.ExecuteQuery();
    }

    public virtual T SaveOrUpdate(T entity)
    {
      Check.Require(!((object) entity is IHasAssignedId<TId>), "For better clarity and reliability, Entities with an assigned Id must call Save or Update");
      this.Session.SaveOrUpdate((object) entity);
      return entity;
    }

    private static NHibernate.LockMode ConvertFrom(Enums.LockMode lockMode)
    {
      FieldInfo field = typeof (NHibernate.LockMode).GetField(((object) lockMode).ToString(), BindingFlags.Static | BindingFlags.Public);
      Check.Ensure(field != (FieldInfo) null, "The provided lock mode , '" + (object) lockMode + ",' could not be translated into an NHibernate.LockMode. This is probably because NHibernate was updated and now has different lock modes which are out of synch with the lock modes maintained in the domain layer.");
      return (NHibernate.LockMode) field.GetValue((object) null);
    }
  }
}
