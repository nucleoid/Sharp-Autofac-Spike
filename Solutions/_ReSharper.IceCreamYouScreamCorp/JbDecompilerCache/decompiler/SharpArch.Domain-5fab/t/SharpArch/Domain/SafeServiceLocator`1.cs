// Type: SharpArch.Domain.SafeServiceLocator`1
// Assembly: SharpArch.Domain, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.Domain.dll

using Microsoft.Practices.ServiceLocation;
using System;

namespace SharpArch.Domain
{
  public static class SafeServiceLocator<TDependency>
  {
    public static TDependency GetService()
    {
      TDependency dependency;
      try
      {
        dependency = (TDependency) ((IServiceProvider) ServiceLocator.get_Current()).GetService(typeof (TDependency));
      }
      catch (NullReferenceException ex)
      {
        throw new NullReferenceException("ServiceLocator has not been initialized; I was trying to retrieve " + (object) typeof (TDependency), (Exception) ex);
      }
      catch (ActivationException ex)
      {
        throw new ActivationException("The needed dependency of type " + typeof (TDependency).Name + " could not be located with the ServiceLocator. You'll need to register it with the Common Service Locator (CSL) via your IoC's CSL adapter. " + ((Exception) ex).Message, (Exception) ex);
      }
      return dependency;
    }
  }
}
