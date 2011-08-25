// Type: SharpArch.NHibernate.SessionFactoryAttribute
// Assembly: SharpArch.NHibernate, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.NHibernate.dll

using System;

namespace SharpArch.NHibernate
{
  [AttributeUsage(AttributeTargets.Class)]
  public class SessionFactoryAttribute : Attribute
  {
    public readonly string FactoryKey;

    public SessionFactoryAttribute(string factoryKey)
    {
      this.FactoryKey = factoryKey;
    }

    public static string GetKeyFrom(object target)
    {
      if (!NHibernateSession.IsConfiguredForMultipleDatabases())
        return NHibernateSession.DefaultFactoryKey;
      object[] customAttributes = target.GetType().GetCustomAttributes(typeof (SessionFactoryAttribute), true);
      if (customAttributes.Length > 0)
        return ((SessionFactoryAttribute) customAttributes[0]).FactoryKey;
      else
        return NHibernateSession.DefaultFactoryKey;
    }
  }
}
