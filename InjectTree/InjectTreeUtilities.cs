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
    /// <typeparam name="TRoot">The type of the root object to create and inject.</typeparam>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    /// <param name="parameters">Optional extra parameters to pass to the constructor and injection process.</param>
    /// </summary>
    public static TRoot CreateInstance<TRoot>(IServiceProvider serviceProvider, params object[] parameters)
        where TRoot : class
    {
        var instance = ActivatorUtilities.CreateInstance<TRoot>(serviceProvider, parameters);
        InjectTree(serviceProvider, instance, parameters);
        return instance;
    }

    /// <summary>
    /// Traverses the tree starting from the root object, injecting properties
    /// and recursively processing branches.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    /// <param name="root">The root object to start tree traversal and injection from.</param>
    /// <param name="parameters">Optional extra parameters to pass to the injection process.</param>
    public static void InjectTree(IServiceProvider serviceProvider, object root, params object[] parameters)
    {
        if (root is null)
            throw new ArgumentNullException(nameof(root));

        var leafPropertyInjectionStrategy = serviceProvider.GetRequiredService<ILeafPropertyInjectionStrategy>();
        var treeTraversalStrategy = serviceProvider.GetRequiredService<ITreeTraversalStrategy>();

        foreach (var node in treeTraversalStrategy.EnumerateNodes(serviceProvider, root))
        {
            leafPropertyInjectionStrategy.Inject(serviceProvider, node, parameters);
        }
    }
}