using System;
using System.Collections.Generic;

namespace InjectTree;

/// <summary>
/// Defines how a tree of injectable objects is traversed.
/// </summary>
public interface ITreeTraversalStrategy
{
    /// <summary>
    /// Enumerates all nodes of a tree starting from the given root.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve branch providers.</param>
    /// <param name="root">The root node.</param>
    /// <returns>An enumerable sequence of all nodes in the tree.</returns>
    IEnumerable<object> EnumerateNodes(IServiceProvider serviceProvider, object root);
}