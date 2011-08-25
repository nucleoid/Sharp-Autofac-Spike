using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Microsoft.Practices.ServiceLocation;

namespace IceCreamYouScreamCorp.Infrastructure
{
    public class AutofacServiceLocator : ServiceLocatorImplBase
    {
        private readonly IContainer _container;

        public AutofacServiceLocator(IContainer container)
        {
            _container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (key != null)
            {
                object objector;
                _container.TryResolveKeyed(key, serviceType, out objector);
                return objector;
            }

            return _container.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>) _container.Resolve(serviceType);
        }
    }
}
