using System;
using System.Reflection;

namespace InjectTree;

internal sealed class NullLeafPropertyInjectionStrategy
{
    public void Inject(object instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        var instanceType = instance.GetType();
        foreach (var property in instanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanWrite)
                continue;

            var attribute = property.GetCustomAttribute<InjectedLeafPropertyAttribute>();
            if (attribute is null)
                continue;

            object? value = null;
            var propertyType = property.PropertyType;
            if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) is null)
                value = Activator.CreateInstance(propertyType);

            property.SetValue(instance, value);
        }
    }
}