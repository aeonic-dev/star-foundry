using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StarFoundry.ECS;

/// <summary>
/// A prefab describes a set of components and (optionally) default values for them; it essentially serves as a template
/// for entities. This is preferable to adding components after creation for performance.<br /><br />
/// Components are initialized lazily, so the first time you use a component on an entity, it will be initialized with
/// the default value from the prefab. If no default value is provided, the default value for the type will be used.
/// </summary>
public sealed class Prefab<TEntity> where TEntity : ComponentEntity<TEntity> {
    public readonly Universe<TEntity> Universe;
    public readonly BitArray ComponentBits;

    // ReSharper disable once StaticMemberInGenericType
    private static readonly ReadOnlyDictionary<Type, object> EmptyDefaults = new(new Dictionary<Type, object>());
    private readonly ReadOnlyDictionary<Type, object> _defaults;

    internal Prefab(Universe<TEntity> universe, BitArray componentBits, ReadOnlyDictionary<Type, object>? defaults) {
        Universe = universe;
        ComponentBits = componentBits;
        _defaults = defaults ?? EmptyDefaults;
    }

    /// <summary>
    /// Gets the default value for a component type in this archetype, or the default value for the type if none is
    /// provided.
    /// </summary>
    public T GetDefaultValue<T>() where T : struct {
        if (_defaults.TryGetValue(typeof(T), out var value)) return (T)value;
        return default;
    }
}