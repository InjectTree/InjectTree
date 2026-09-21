# InjectTree

Recursive property injection for object trees — designed for WinForms, works with any object graph.

[![License: BSD-3-Clause](https://img.shields.io/badge/License-BSD--3--Clause-blue.svg)](LICENSE)

## Why

Constructor injection doesn't work well with designer-generated types like WinForms `Form`s and `UserControl`s — you don't control how they're instantiated. **InjectTree** solves this with **property injection over a tree**: it walks an object graph (e.g. a form and all its child controls), and for every node it fills properties marked with `[InjectedLeafProperty]` from an `IServiceProvider`.

- No constructor changes required.
- Works recursively — inject an entire control tree in one call.
- Built on `Microsoft.Extensions.DependencyInjection`, so it fits into a standard DI setup.
- Targets **.NET Framework 4.8** and **.NET 10.0**.

## Install

```bash
dotnet add package InjectTree
```

For WinForms-specific helpers, also add:

```bash
dotnet add package InjectTree.WinForms
```

## Quick start

**1. Mark the properties you want injected:**

```csharp
public partial class MainForm : Form
{
    [InjectedLeafProperty]
    public ILogger Logger { get; set; }

    [InjectedLeafProperty(isRequired: false)]
    public IOptionalService Optional { get; set; }
}
```

**2. Register InjectTree and your services:**

```csharp
var services = new ServiceCollection();

services.AddInjectTree();
services.AddSingleton<ILogger, ConsoleLogger>();

// Register MainForm so it gets created + injected in one call
services.AddTreeSingleton<MainForm>();

var provider = services.BuildServiceProvider();
var form = provider.GetRequiredService<MainForm>();
```

**3. Or inject an existing instance directly:**

```csharp
var form = new MainForm();
InjectTreeUtilities.InjectTree(form, provider);
```

## How the tree is walked

By default, InjectTree only knows how to walk objects that implement `IInjectedTreeNode`:

```csharp
public partial class MainForm : Form, IInjectedTreeNode
{
    public IEnumerable GetBranches() => Controls; // or any custom child collection
}
```

If you don't want to implement an interface on every node, register a custom `IBranchProvider` instead — for example, to walk WinForms' `Controls` collection automatically for every `Control`:

```csharp
services.AddBranchProvider<Control>(control => control.Controls);
```

Multiple branch providers can be registered together; InjectTree merges them during traversal and de-duplicates nodes automatically (cycle-safe).

## Resolving values

For each `[InjectedLeafProperty]`, InjectTree resolves a value in this order:

1. The first matching object passed as an extra `parameters` argument.
2. `serviceProvider.GetService(propertyType)`.

If nothing resolves and the property is required (the default), an exception is thrown. Mark a property as optional to skip silently instead:

```csharp
[InjectedLeafProperty(isRequired: false)]
public IOptionalService Optional { get; set; }
```

## API overview

| Type | Purpose |
|---|---|
| `InjectedLeafPropertyAttribute` | Marks a property as injectable; `IsRequired` defaults to `true`. |
| `IInjectedTreeNode` | Opt-in interface for nodes that expose their own children via `GetBranches()`. |
| `IBranchProvider` | Custom strategy to discover children of any node type, without implementing an interface on it. |
| `ITreeTraversalStrategy` / `DefaultTreeTraversalStrategy` | Controls how the tree is walked (default: breadth-first, cycle-safe). |
| `ILeafPropertyInjectionStrategy` / `DefaultLeafPropertyInjectionStrategy` | Controls how each node's properties are resolved and set. |
| `InjectTreeUtilities` | Static entry points: `CreateInstance<T>`, `InjectTree`, `InjectTreeWithNull`. |
| `ServiceCollectionTreeExtensions` | DI registration helpers: `AddInjectTree`, `AddBranchProvider`, `AddTree{Singleton,Scoped,Transient}`. |

Both traversal and injection strategies are registered as regular DI services, so you can swap in your own implementation of `ITreeTraversalStrategy` or `ILeafPropertyInjectionStrategy` if the defaults don't fit your scenario.

## Resetting a tree

To clear all injected properties back to their default value (e.g. before disposing a form) without touching the DI container:

```csharp
InjectTreeUtilities.InjectTreeWithNull(form, provider);
```

## License

[BSD-3-Clause](LICENSE)
