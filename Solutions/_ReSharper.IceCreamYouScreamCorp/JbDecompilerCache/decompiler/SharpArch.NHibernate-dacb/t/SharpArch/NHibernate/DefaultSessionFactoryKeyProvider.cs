// Type: SharpArch.NHibernate.DefaultSessionFactoryKeyProvider
// Assembly: SharpArch.NHibernate, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.NHibernate.dll

namespace SharpArch.NHibernate
{
  public class DefaultSessionFactoryKeyProvider : ISessionFactoryKeyProvider
  {
    public string GetKey()
    {
      return NHibernateSession.DefaultFactoryKey;
    }

    public string GetKeyFrom(object anObject)
    {
      return SessionFactoryAttribute.GetKeyFrom(anObject);
    }
  }
}
