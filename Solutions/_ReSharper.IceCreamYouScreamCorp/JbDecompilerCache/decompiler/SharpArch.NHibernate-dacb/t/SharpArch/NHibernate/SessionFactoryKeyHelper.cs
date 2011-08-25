// Type: SharpArch.NHibernate.SessionFactoryKeyHelper
// Assembly: SharpArch.NHibernate, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.NHibernate.dll

using SharpArch.Domain;

namespace SharpArch.NHibernate
{
  public static class SessionFactoryKeyHelper
  {
    public static string GetKey()
    {
      return SafeServiceLocator<ISessionFactoryKeyProvider>.GetService().GetKey();
    }

    public static string GetKeyFrom(object anObject)
    {
      return SafeServiceLocator<ISessionFactoryKeyProvider>.GetService().GetKeyFrom(anObject);
    }
  }
}
