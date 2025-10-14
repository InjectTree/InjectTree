using System;
using System.Collections;

namespace InjectTree;

public class InjectableNodeBranchProvider : IBranchProvider
{
    public IEnumerable GetBranches(object node)
    {
        return node is IInjectableNode injectableNode ? injectableNode.GetBranches() : Array.Empty<object>();
    }
}