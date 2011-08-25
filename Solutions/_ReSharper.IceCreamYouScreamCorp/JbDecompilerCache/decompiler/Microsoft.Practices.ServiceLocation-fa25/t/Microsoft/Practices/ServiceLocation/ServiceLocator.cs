// Type: Microsoft.Practices.ServiceLocation.ServiceLocator
// Assembly: Microsoft.Practices.ServiceLocation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\Packages\CommonServiceLocator.1.0\lib\NET35\Microsoft.Practices.ServiceLocation.dll

namespace Microsoft.Practices.ServiceLocation
{
  public static class ServiceLocator
  {
    private static ServiceLocatorProvider currentProvider;

    public static IServiceLocator Current
    {
      get
      {
        return ServiceLocator.currentProvider();
      }
    }

    public static void SetLocatorProvider(ServiceLocatorProvider newProvider)
    {
      ServiceLocator.currentProvider = newProvider;
    }
  }
}
