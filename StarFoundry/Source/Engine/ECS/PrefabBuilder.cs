using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StarFoundry.Misc;

namespace StarFoundry.Engine.ECS;

/// <summary>
/// A builder for a <see cref="Prefab{TEntity}"/> that allows you to specify which components it should have and what
/// their default values should be.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class PrefabBuilder<TEntity> where TEntity : ComponentEntity<TEntity> {
    private readonly Universe<TEntity> _universe;
    private readonly BitArray _componentBits;
    private readonly Dictionary<Type, object> _defaults;

    public PrefabBuilder(Universe<TEntity> universe,
        BitArray? componentBits = null, Dictionary<Type, object>? defaults = null) {
        _universe = universe;
        _componentBits = componentBits == null ? new BitArray(64) : new BitArray(componentBits);
        _defaults = defaults ?? new Dictionary<Type, object>();
    }

    /// <summary>
    /// Returns this builder with the given component type added to it.
    /// </summary>
    public PrefabBuilder<TEntity> WithComponent<T>(Func<T> defaultValue) where T : struct {
        return WithComponent(defaultValue());
    }

    /// <summary>
    /// Returns this builder with the given component type added to it.
    /// </summary>
    public PrefabBuilder<TEntity> WithComponent<T>(T defaultValue = default) where T : struct {
        var index = _universe.GetComponentTypeIndex<T>();
        _componentBits.EnsureLength(index + 1);
        _componentBits[index] = true;

        _defaults[typeof(T)] = defaultValue;

        return this;
    }

    /// <summary>
    /// Builds the prefab.
    /// </summary>
    public Prefab<TEntity> Build() {
        return new Prefab<TEntity>(_universe, _componentBits, new ReadOnlyDictionary<Type, object>(_defaults));
    }
}