using System;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace InjectTree;

/// <summary>
/// Extension methods for IServiceCollection to register services using InjectTreeUtilities.
/// </summary>
public static class ServiceCollectionTreeExtensions
{
    /// <summary>
    /// Registers a singleton <see cref="IBranchProvider"/> for the specified root type, using the provided function to retrieve branches (children) from a node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the branch provider to.</param>
    /// <param name="getBranchesFunc">A function that returns the branches (children) for a given node of type <typeparamref name="TRoot"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddBranchProvider<TRoot>(this IServiceCollection services, Func<TRoot, IEnumerable> getBranchesFunc)
    {
        return services.AddSingleton<IBranchProvider>(new TypedBranchProvider<TRoot>(getBranchesFunc));
    }


    /// <summary>
    /// Register default services for InjectTree.
    /// </summary>
    /// <param name="services">The IServiceCollection to add InjectTree to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddInjectTree(this IServiceCollection services)
    {
        return services
            .AddSingleton<ILeafPropertyInjectionStrategy, DefaultLeafPropertyInjectionStrategy>()
            .AddSingleton<ITreeTraversalStrategy, DefaultTreeTraversalStrategy>()
            .AddBranchProvider<IInjectedTreeNode>(n => n.GetBranches());
    }

    /// <summary>
    /// Registers a singleton service of type TRoot using InjectTreeUtilities for instantiation.
    /// </summary>
    /// <typeparam name="TRoot">The type of the service to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeSingleton<TRoot>(this IServiceCollection services)
        where TRoot : class
    {
        return services.AddSingleton<TRoot>(sp => InjectTreeUtilities.CreateInstance<TRoot>(sp));
    }

    /// <summary>
    /// Registers a singleton service of type TService with implementation type TImplementation using InjectTreeUtilities.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type to instantiate.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddSingleton<TService>(sp => InjectTreeUtilities.CreateInstance<TImplementation>(sp));
    }

    /// <summary>
    /// Registers a scoped service of type TRoot using InjectTreeUtilities for instantiation.
    /// </summary>
    /// <typeparam name="TRoot">The type of the service to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeScoped<TRoot>(this IServiceCollection services)
        where TRoot : class
    {
        return services.AddScoped<TRoot>(sp => InjectTreeUtilities.CreateInstance<TRoot>(sp));
    }

    /// <summary>
    /// Registers a scoped service of type TService with implementation type TImplementation using InjectTreeUtilities.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type to instantiate.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddScoped<TService>(sp => InjectTreeUtilities.CreateInstance<TImplementation>(sp));
    }

    /// <summary>
    /// Registers a transient service of type TRoot using InjectTreeUtilities for instantiation.
    /// </summary>
    /// <typeparam name="TRoot">The type of the service to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeTransient<TRoot>(this IServiceCollection services)
        where TRoot : class
    {
        return services.AddTransient<TRoot>(sp => InjectTreeUtilities.CreateInstance<TRoot>(sp));
    }

    /// <summary>
    /// Registers a transient service of type TService with implementation type TImplementation using InjectTreeUtilities.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type to instantiate.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddTransient<TService>(sp => InjectTreeUtilities.CreateInstance<TImplementation>(sp));
    }
}