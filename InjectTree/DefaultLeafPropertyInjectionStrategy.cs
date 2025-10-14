using System;
using System.Linq;
using System.Reflection;

namespace InjectTree;

public sealed class DefaultLeafPropertyInjectionStrategy : ILeafPropertyInjectionStrategy
{
    public void Inject(IServiceProvider serviceProvider, object instance, params object[] parameters)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        if (serviceProvider is null)
            throw new ArgumentNullException(nameof(serviceProvider));

        var instanceType = instance.GetType();
        foreach (var property in instanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanWrite)
                continue;

            var attribute = property.GetCustomAttribute<InjectedLeafPropertyAttribute>();
            if (attribute is null)
                continue;

            var value = parameters.FirstOrDefault(parameter => property.PropertyType.IsInstanceOfType(parameter)) ??
                        serviceProvider.GetService(property.PropertyType);

            if (value is null && attribute.IsRequired)
                throw new InvalidOperationException(
                    $"Required property '{property.Name}' of type '{property.PropertyType}' " +
                    $"on instance '{instanceType.FullName}' could not be resolved.");

            if (value is not null)
                property.SetValue(instance, value);
        }
    }
}