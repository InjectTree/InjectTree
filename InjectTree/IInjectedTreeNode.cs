using System.Collections;

namespace InjectTree;

/// <summary>
/// Implement this interface to mark an object as a node in an inject tree.
/// </summary>
public interface IInjectedTreeNode
{
    /// <summary>
    /// Returns the branches (child objects) of this node.
    /// </summary>
    /// <returns>An <see cref="IEnumerable"/> of child nodes.</returns>
    IEnumerable GetBranches();
}