using Microsoft.Extensions.DependencyInjection;

namespace InjectTree;

/// <summary>
/// Extension methods for IServiceCollection to register services using InjectTreeUtilities.
/// </summary>
public static class ServiceCollectionTreeExtensions
{
    /// <summary>
    /// Register default services for InjectTree.
    /// </summary>
    /// <param name="services">The IServiceCollection to add InjectTree to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddInjectTree(this IServiceCollection services)
    {
        return services
            .AddSingleton<IBranchProvider, InjectableNodeBranchProvider>()
            .AddSingleton<ILeafPropertyInjectionStrategy, DefaultLeafPropertyInjectionStrategy>()
            .AddSingleton<ITreeTraversalStrategy, DefaultTreeTraversalStrategy>();
    }

    /// <summary>
    /// Registers a singleton service of type T using InjectTreeUtilities for instantiation.
    /// </summary>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeSingleton<T>(this IServiceCollection services)
        where T : class
    {
        // Register T as a singleton, using InjectTreeUtilities to create the instance
        return services.AddSingleton<T>(sp => InjectTreeUtilities.CreateInstance<T>(sp));
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
    /// Registers a scoped service of type T using InjectTreeUtilities for instantiation.
    /// </summary>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeScoped<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddScoped<T>(sp => InjectTreeUtilities.CreateInstance<T>(sp));
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
    /// Registers a transient service of type T using InjectTreeUtilities for instantiation.
    /// </summary>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddTreeTransient<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddTransient<T>(sp => InjectTreeUtilities.CreateInstance<T>(sp));
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