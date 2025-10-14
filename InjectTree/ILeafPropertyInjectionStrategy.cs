using System;

namespace InjectTree;

/// <summary>
/// Defines a strategy for injecting properties into leaf object instances within an object graph.
/// </summary>
public interface ILeafPropertyInjectionStrategy
{
    /// <summary>
    /// Injects properties into a leaf object instance.
    /// </summary>
    /// <param name="instance">The target instance.</param>
    /// <param name="serviceProvider">The service provider used for injection.</param>
    /// <param name="parameters">Optional parameters for the injection process.</param>
    void Inject(object instance, IServiceProvider serviceProvider, params object[] parameters);
}