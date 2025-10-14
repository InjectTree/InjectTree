using System.Collections;

namespace InjectTree;

/// <summary>
/// Represents a node in the injection tree that can explicitly expose its
/// injectable branches (child nodes).
/// </summary>
public interface IInjectableNode
{
    /// <summary>
    /// Returns the child objects that represent injectable branches of this node.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable"/> of child objects that should be traversed by the injection tree walker.
    /// </returns>
    IEnumerable GetBranches();
}