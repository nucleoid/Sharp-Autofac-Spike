// Type: SharpArch.NHibernate.NHibernateSession
// Assembly: SharpArch.NHibernate, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.NHibernate.dll

using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Validator.Engine;
using SharpArch.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SharpArch.NHibernate
{
  public static class NHibernateSession
  {
    public static readonly string DefaultFactoryKey = "nhibernate.current_session";
    private static readonly Dictionary<string, ISessionFactory> SessionFactories = new Dictionary<string, ISessionFactory>();
    private static IInterceptor registeredInterceptor;
    private static INHibernateConfigurationCache configurationCache;

    public static INHibernateConfigurationCache ConfigurationCache
    {
      get
      {
        return NHibernateSession.configurationCache;
      }
      set
      {
        if (NHibernateSession.Storage != null)
          throw new InvalidOperationException("Cannot set the ConfigurationCache property after calling Init");
        NHibernateSession.configurationCache = value;
      }
    }

    public static ISession Current
    {
      get
      {
        Check.Require(!NHibernateSession.IsConfiguredForMultipleDatabases(), "The NHibernateSession.Current property may only be invoked if you only have one NHibernate session factory; i.e., you're only communicating with one database.  Since you're configured communications with multiple databases, you should instead call CurrentFor(string factoryKey)");
        return NHibernateSession.CurrentFor(NHibernateSession.DefaultFactoryKey);
      }
    }

    public static ISessionStorage Storage { get; set; }

    public static ValidatorEngine ValidatorEngine { get; set; }

    static NHibernateSession()
    {
    }

    [CLSCompliant(false)]
    public static Configuration AddConfiguration(string factoryKey, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile, IDictionary<string, string> cfgProperties, string validatorCfgFile, IPersistenceConfigurer persistenceConfigurer)
    {
      INHibernateConfigurationCache configurationCache = NHibernateSession.ConfigurationCache;
      if (configurationCache != null)
      {
        Configuration cfg = configurationCache.LoadConfiguration(factoryKey, cfgFile, mappingAssemblies);
        if (cfg != null)
          return NHibernateSession.AddConfiguration(factoryKey, cfg.BuildSessionFactory(), cfg, validatorCfgFile);
      }
      Configuration config = NHibernateSession.AddConfiguration(factoryKey, mappingAssemblies, autoPersistenceModel, NHibernateSession.ConfigureNHibernate(cfgFile, cfgProperties), validatorCfgFile, persistenceConfigurer);
      if (configurationCache != null)
        configurationCache.SaveConfiguration(factoryKey, config);
      return config;
    }

    [CLSCompliant(false)]
    public static Configuration AddConfiguration(string factoryKey, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, Configuration cfg, string validatorCfgFile, IPersistenceConfigurer persistenceConfigurer)
    {
      ISessionFactory sessionFactoryFor = NHibernateSession.CreateSessionFactoryFor((IEnumerable<string>) mappingAssemblies, autoPersistenceModel, cfg, persistenceConfigurer);
      return NHibernateSession.AddConfiguration(factoryKey, sessionFactoryFor, cfg, validatorCfgFile);
    }

    [CLSCompliant(false)]
    public static Configuration AddConfiguration(string factoryKey, ISessionFactory sessionFactory, Configuration cfg, string validatorCfgFile)
    {
      Check.Require(!NHibernateSession.SessionFactories.ContainsKey(factoryKey), "A session factory has already been configured with the key of " + factoryKey);
      NHibernateSession.SessionFactories.Add(factoryKey, sessionFactory);
      return cfg;
    }

    public static void CloseAllSessions()
    {
      if (NHibernateSession.Storage == null)
        return;
      using (IEnumerator<ISession> enumerator = NHibernateSession.Storage.GetAllSessions().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          ISession current = enumerator.Current;
          if (current.get_IsOpen())
            current.Close();
        }
      }
    }

    public static ISession CurrentFor(string factoryKey)
    {
      Check.Require(!string.IsNullOrEmpty(factoryKey), "factoryKey may not be null or empty");
      Check.Require(NHibernateSession.Storage != null, "An ISessionStorage has not been configured");
      Check.Require(NHibernateSession.SessionFactories.ContainsKey(factoryKey), "An ISessionFactory does not exist with a factory key of " + factoryKey);
      ISession session = NHibernateSession.Storage.GetSessionForKey(factoryKey);
      if (session == null)
      {
        session = NHibernateSession.registeredInterceptor == null ? NHibernateSession.SessionFactories[factoryKey].OpenSession() : NHibernateSession.SessionFactories[factoryKey].OpenSession(NHibernateSession.registeredInterceptor);
        NHibernateSession.Storage.SetSessionForKey(factoryKey, session);
      }
      return session;
    }

    public static ISessionFactory GetDefaultSessionFactory()
    {
      return NHibernateSession.GetSessionFactoryFor(NHibernateSession.DefaultFactoryKey);
    }

    public static ISessionFactory GetSessionFactoryFor(string factoryKey)
    {
      if (!NHibernateSession.SessionFactories.ContainsKey(factoryKey))
        return (ISessionFactory) null;
      else
        return NHibernateSession.SessionFactories[factoryKey];
    }

    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, (AutoPersistenceModel) null, (string) null, (IDictionary<string, string>) null, (string) null, (IPersistenceConfigurer) null);
    }

    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, (AutoPersistenceModel) null, cfgFile, (IDictionary<string, string>) null, (string) null, (IPersistenceConfigurer) null);
    }

    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, IDictionary<string, string> cfgProperties)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, (AutoPersistenceModel) null, (string) null, cfgProperties, (string) null, (IPersistenceConfigurer) null);
    }

    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile, string validatorCfgFile)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, (AutoPersistenceModel) null, cfgFile, (IDictionary<string, string>) null, validatorCfgFile, (IPersistenceConfigurer) null);
    }

    [CLSCompliant(false)]
    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, autoPersistenceModel, (string) null, (IDictionary<string, string>) null, (string) null, (IPersistenceConfigurer) null);
    }

    [CLSCompliant(false)]
    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, (IDictionary<string, string>) null, (string) null, (IPersistenceConfigurer) null);
    }

    [CLSCompliant(false)]
    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, IDictionary<string, string> cfgProperties)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, autoPersistenceModel, (string) null, cfgProperties, (string) null, (IPersistenceConfigurer) null);
    }

    [CLSCompliant(false)]
    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile, string validatorCfgFile)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, (IDictionary<string, string>) null, validatorCfgFile, (IPersistenceConfigurer) null);
    }

    [CLSCompliant(false)]
    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile, IDictionary<string, string> cfgProperties, string validatorCfgFile)
    {
      return NHibernateSession.Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, cfgProperties, validatorCfgFile, (IPersistenceConfigurer) null);
    }

    [CLSCompliant(false)]
    public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile, IDictionary<string, string> cfgProperties, string validatorCfgFile, IPersistenceConfigurer persistenceConfigurer)
    {
      NHibernateSession.InitStorage(storage);
      try
      {
        return NHibernateSession.AddConfiguration(NHibernateSession.DefaultFactoryKey, mappingAssemblies, autoPersistenceModel, cfgFile, cfgProperties, validatorCfgFile, persistenceConfigurer);
      }
      catch
      {
        NHibernateSession.Storage = (ISessionStorage) null;
        throw;
      }
    }

    public static void InitStorage(ISessionStorage storage)
    {
      Check.Require(storage != null, "storage mechanism was null but must be provided");
      Check.Require(NHibernateSession.Storage == null, "A storage mechanism has already been configured for this application");
      NHibernateSession.Storage = storage;
    }

    public static bool IsConfiguredForMultipleDatabases()
    {
      return NHibernateSession.SessionFactories.Count > 1;
    }

    public static void RegisterInterceptor(IInterceptor interceptor)
    {
      Check.Require(interceptor != null, "interceptor may not be null");
      NHibernateSession.registeredInterceptor = interceptor;
    }

    public static void RemoveSessionFactoryFor(string factoryKey)
    {
      if (NHibernateSession.GetSessionFactoryFor(factoryKey) == null)
        return;
      NHibernateSession.SessionFactories.Remove(factoryKey);
    }

    public static void Reset()
    {
      if (NHibernateSession.Storage != null)
      {
        using (IEnumerator<ISession> enumerator = NHibernateSession.Storage.GetAllSessions().GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            ((IDisposable) enumerator.Current).Dispose();
        }
      }
      NHibernateSession.SessionFactories.Clear();
      NHibernateSession.Storage = (ISessionStorage) null;
      NHibernateSession.registeredInterceptor = (IInterceptor) null;
      NHibernateSession.ValidatorEngine = (ValidatorEngine) null;
      NHibernateSession.ConfigurationCache = (INHibernateConfigurationCache) null;
    }

    private static Configuration ConfigureNHibernate(string cfgFile, IDictionary<string, string> cfgProperties)
    {
      Configuration configuration = new Configuration();
      if (cfgProperties != null)
        configuration.AddProperties(cfgProperties);
      if (!string.IsNullOrEmpty(cfgFile))
        return configuration.Configure(cfgFile);
      if (File.Exists("Hibernate.cfg.xml"))
        return configuration.Configure();
      else
        return configuration;
    }

    private static ISessionFactory CreateSessionFactoryFor(IEnumerable<string> mappingAssemblies, AutoPersistenceModel autoPersistenceModel, Configuration cfg, IPersistenceConfigurer persistenceConfigurer)
    {
      FluentConfiguration fluentConfiguration = Fluently.Configure(cfg);
      if (persistenceConfigurer != null)
        fluentConfiguration.Database(persistenceConfigurer);
      fluentConfiguration.Mappings((Action<MappingConfiguration>) (m =>
      {
        foreach (string item_0 in mappingAssemblies)
        {
          Assembly local_1 = Assembly.LoadFrom(NHibernateSession.MakeLoadReadyAssemblyName(item_0));
          m.get_HbmMappings().AddFromAssembly(local_1);
          m.get_FluentMappings().AddFromAssembly(local_1).get_Conventions().AddAssembly(local_1);
        }
        if (autoPersistenceModel == null)
          return;
        m.get_AutoMappings().Add(autoPersistenceModel);
      }));
      return fluentConfiguration.BuildSessionFactory();
    }

    private static string MakeLoadReadyAssemblyName(string assemblyName)
    {
      return assemblyName.IndexOf(".dll") == -1 ? assemblyName.Trim() + ".dll" : assemblyName.Trim();
    }
  }
}
