using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace InjectTree;

public sealed class DefaultTreeTraversalStrategy : ITreeTraversalStrategy
{
    public IEnumerable EnumerateNodes(object root, IServiceProvider serviceProvider)
    {
        if (root is null)
            throw new ArgumentNullException(nameof(root));

        if (serviceProvider is null)
            throw new ArgumentNullException(nameof(serviceProvider));

        var branchProviders = serviceProvider.GetServices<IBranchProvider>().ToArray();

        var discovered = new HashSet<object>();
        var queue = new Queue<object>();

        discovered.Add(root);
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var branchProvider in branchProviders)
            {
                foreach (var branch in branchProvider.GetBranches(current))
                {
                    if (discovered.Add(branch))
                        queue.Enqueue(branch);
                }
            }
        }
    }

}
