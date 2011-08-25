// Type: CommonServiceLocator.WindsorAdapter.WindsorServiceLocator
// Assembly: CommonServiceLocator.WindsorAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\Packages\CommonServiceLocator.WindsorAdapter.1.0\lib\NET35\CommonServiceLocator.WindsorAdapter.dll

using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;

namespace CommonServiceLocator.WindsorAdapter
{
  public class WindsorServiceLocator : ServiceLocatorImplBase
  {
    private readonly IWindsorContainer container;

    public WindsorServiceLocator(IWindsorContainer container)
    {
      base.\u002Ector();
      this.container = container;
    }

    protected virtual object DoGetInstance(Type serviceType, string key)
    {
      if (key != null)
        return this.container.Resolve(key, serviceType);
      else
        return this.container.Resolve(serviceType);
    }

    protected virtual IEnumerable<object> DoGetAllInstances(Type serviceType)
    {
      return (IEnumerable<object>) this.container.ResolveAll(serviceType);
    }
  }
}
