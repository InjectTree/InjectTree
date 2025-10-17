using System;
using Microsoft.Extensions.DependencyInjection;

namespace InjectTree;

/// <summary>
/// Provides utility methods to create and inject object trees marked with
/// <see cref="InjectedLeafPropertyAttribute"/>.
/// </summary>
public static class InjectTreeUtilities
{
    /// <summary>
    /// Creates an instance of the specified type and recursively injects all
    /// injectable leaves within its object tree, using both the service provider
    /// and optional extra parameters.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root object to create and inject.</typeparam>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    /// <param name="parameters">Optional extra parameters to pass to the constructor and injection process.</param>
    /// <returns>
    /// An instance of <typeparamref name="TRoot"/> with all injectable leaves in its object tree injected.
    /// </returns>
    public static TRoot CreateInstance<TRoot>(IServiceProvider serviceProvider, params object[] parameters)
        where TRoot : class
    {
        if (serviceProvider is null)
            throw new ArgumentNullException(nameof(serviceProvider));

        if (parameters is null)
            throw new ArgumentNullException(nameof(parameters));

        var instance = ActivatorUtilities.CreateInstance<TRoot>(serviceProvider, parameters);
        InjectTree(instance, serviceProvider, parameters);
        return instance;
    }

    /// <summary>
    /// Traverses the tree starting from the root object, injecting properties
    /// and recursively processing branches.
    /// </summary>
    /// <param name="root">The root object to start tree traversal and injection from.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    /// <param name="parameters">Optional extra parameters to pass to the injection process.</param>
    public static void InjectTree(object root, IServiceProvider serviceProvider, params object[] parameters)
    {
        if (root is null)
            throw new ArgumentNullException(nameof(root));

        if (serviceProvider is null)
            throw new ArgumentNullException(nameof(serviceProvider));

        if (parameters is null)
            throw new ArgumentNullException(nameof(parameters));

        var leafPropertyInjectionStrategy = serviceProvider.GetRequiredService<ILeafPropertyInjectionStrategy>();
        var treeTraversalStrategy = serviceProvider.GetRequiredService<ITreeTraversalStrategy>();

        foreach (var node in treeTraversalStrategy.EnumerateNodes(root, serviceProvider))
        {
            leafPropertyInjectionStrategy.Inject(node, serviceProvider, parameters);
        }
    }

    /// <summary>
    /// Traverses the tree starting from the root object and injects null values into
    /// the injectable leaves. This uses the registered <see cref="ITreeTraversalStrategy"/>.
    /// </summary>
    /// <param name="root">The root object to start tree traversal from.</param>
    /// <param name="serviceProvider">The service provider used to resolve the traversal strategy.</param>
    public static void InjectTreeWithNull(object root, IServiceProvider serviceProvider)
    {
        if (root is null)
            throw new ArgumentNullException(nameof(root));

        if (serviceProvider is null)
            throw new ArgumentNullException(nameof(serviceProvider));

        var nullLeafPropertyInjectionStrategy = new NullLeafPropertyInjectionStrategy();
        var treeTraversalStrategy = serviceProvider.GetRequiredService<ITreeTraversalStrategy>();

        foreach (var node in treeTraversalStrategy.EnumerateNodes(root, serviceProvider))
        {
            nullLeafPropertyInjectionStrategy.Inject(node);
        }
    }
}