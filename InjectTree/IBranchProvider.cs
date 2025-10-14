using System.Collections;

namespace InjectTree;

/// <summary>
/// Provides the branches (children) of a node in an object tree.
/// </summary>
public interface IBranchProvider
{
    /// <summary>
    /// Returns the branches (child objects) of a given node.
    /// </summary>
    /// <param name="node">The parent node.</param>
    /// <returns>An <see cref="IEnumerable"/> of child nodes.</returns>
    IEnumerable GetBranches(object node);
}
