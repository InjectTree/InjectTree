using System;

namespace InjectTree;

/// <summary>
/// Attribute to mark a property as an injected leaf property in the dependency injection tree.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class InjectedLeafPropertyAttribute : Attribute
{
    public InjectedLeafPropertyAttribute() : this(true)
    {
    }

    public InjectedLeafPropertyAttribute(bool isRequired)
    {
        IsRequired = isRequired;
    }

    /// <summary>
    /// Indicates whether the property is required. Defaults to true.
    /// </summary>
    public bool IsRequired { get; }
}
