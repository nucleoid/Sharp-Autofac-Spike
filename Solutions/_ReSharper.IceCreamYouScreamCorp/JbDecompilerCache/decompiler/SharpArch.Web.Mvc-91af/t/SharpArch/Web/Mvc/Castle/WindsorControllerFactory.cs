// Type: SharpArch.Web.Mvc.Castle.WindsorControllerFactory
// Assembly: SharpArch.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.Web.Mvc.dll

using Castle.Windsor;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SharpArch.Web.Mvc.Castle
{
  public class WindsorControllerFactory : DefaultControllerFactory
  {
    private IWindsorContainer container;

    public WindsorControllerFactory(IWindsorContainer container)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      this.container = container;
    }

    public override void ReleaseController(IController controller)
    {
      IDisposable disposable = controller as IDisposable;
      if (disposable != null)
        disposable.Dispose();
      this.container.Release((object) controller);
    }

    protected override IController GetControllerInstance(RequestContext context, Type controllerType)
    {
      if (controllerType == (Type) null)
        throw new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", (object) context.HttpContext.Request.Path));
      else
        return (IController) this.container.Resolve(controllerType);
    }
  }
}
