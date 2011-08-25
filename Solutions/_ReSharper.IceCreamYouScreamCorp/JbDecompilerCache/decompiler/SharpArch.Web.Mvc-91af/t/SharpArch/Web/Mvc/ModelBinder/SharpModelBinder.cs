// Type: SharpArch.Web.Mvc.ModelBinder.SharpModelBinder
// Assembly: SharpArch.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\statebank\Spike\IceCreamYouScreamCorp\ReferencedAssemblies\SharpArchitecture\SharpArch.Web.Mvc.dll

using Iesi.Collections.Generic;
using SharpArch.Domain.DomainModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SharpArch.Web.Mvc.ModelBinder
{
  public class SharpModelBinder : DefaultModelBinder
  {
    private const string IdPropertyName = "Id";

    protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
    {
      Type propertyType = propertyDescriptor.PropertyType;
      if (SharpModelBinder.IsEntityType(propertyType))
        return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, (IModelBinder) new EntityValueBinder());
      if (SharpModelBinder.IsSimpleGenericBindableEntityCollection(propertyType))
        return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, (IModelBinder) new EntityCollectionValueBinder());
      else
        return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
    }

    protected override bool OnModelUpdating(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      if (SharpModelBinder.IsEntityType(bindingContext.ModelType))
      {
        PropertyDescriptor propertyDescriptor = Enumerable.SingleOrDefault<PropertyDescriptor>(Enumerable.Where<PropertyDescriptor>(Enumerable.Cast<PropertyDescriptor>((IEnumerable) TypeDescriptor.GetProperties(bindingContext.ModelType)), (Func<PropertyDescriptor, bool>) (property => property.Name == "Id")));
        this.BindProperty(controllerContext, bindingContext, propertyDescriptor);
      }
      return base.OnModelUpdating(controllerContext, bindingContext);
    }

    protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
    {
      if (propertyDescriptor.Name == "Id")
        SharpModelBinder.SetIdProperty(bindingContext, propertyDescriptor, value);
      else if (value is IEnumerable && SharpModelBinder.IsSimpleGenericBindableEntityCollection(propertyDescriptor.PropertyType))
        SharpModelBinder.SetEntityCollectionProperty(bindingContext, propertyDescriptor, value);
      else
        base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
    }

    private static bool IsEntityType(Type propertyType)
    {
      return Enumerable.Any<Type>((IEnumerable<Type>) propertyType.GetInterfaces(), (Func<Type, bool>) (type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEntityWithTypedId<>)));
    }

    private static bool IsSimpleGenericBindableEntityCollection(Type propertyType)
    {
      return propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof (IList<>) || propertyType.GetGenericTypeDefinition() == typeof (ICollection<>) || propertyType.GetGenericTypeDefinition() == typeof (ISet<>) || propertyType.GetGenericTypeDefinition() == typeof (IEnumerable<>)) && SharpModelBinder.IsEntityType(Enumerable.First<Type>((IEnumerable<Type>) propertyType.GetGenericArguments()));
    }

    private static void SetEntityCollectionProperty(ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
    {
      object target = propertyDescriptor.GetValue(bindingContext.Model);
      if (target == value)
        return;
      Type type = target.GetType();
      foreach (object obj in value as IEnumerable)
        type.InvokeMember("Add", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, target, new object[1]
        {
          obj
        });
    }

    private static void SetIdProperty(ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
    {
      Type propertyType = propertyDescriptor.PropertyType;
      object obj = value != null ? Convert.ChangeType(value, propertyType) : (propertyType.IsValueType ? Activator.CreateInstance(propertyType) : (object) null);
      (bindingContext.ModelType.GetProperty(propertyDescriptor.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) ?? bindingContext.ModelType.GetProperty(propertyDescriptor.Name, BindingFlags.Instance | BindingFlags.Public)).SetValue(bindingContext.Model, obj, (object[]) null);
    }
  }
}
