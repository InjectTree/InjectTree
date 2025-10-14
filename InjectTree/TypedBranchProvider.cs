using System;
using System.Collections;

namespace InjectTree;

internal sealed class TypedBranchProvider<TRoot> : IBranchProvider
{
    private readonly Func<TRoot, IEnumerable> _getBranchesFunc;

    public TypedBranchProvider(Func<TRoot, IEnumerable> getBranchesFunc)
    {
        _getBranchesFunc = getBranchesFunc;
    }

    public IEnumerable GetBranches(object node)
    {
        return node is TRoot root ? _getBranchesFunc(root) : Array.Empty<object>();
    }
}