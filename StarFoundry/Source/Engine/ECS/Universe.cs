using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MonoGame.Extended.Collections;
using StarFoundry.Misc;

namespace StarFoundry.Engine.ECS;

/// <summary>
/// A universe is a collection of entities and systems, with a base entity type. It is the root of the ECS.
/// </summary>
public class Universe<TEntity> /*: IDisposable*/ where TEntity : ComponentEntity<TEntity> {
    private readonly Bag<TEntity> _entities = new();
    private readonly IndexPool _entityIndexPool = new();

    private readonly Bag<IComponentBag> _componentBags = new();
    private readonly Dictionary<Type, int> _typeToIndex = new();

    /// <summary>
    /// Adds entities to the universe, initializing them.
    /// </summary>
    /// <param name="entities"></param>
    public void AddEntities(params TEntity[] entities) => AddEntities((IEnumerable<TEntity>)entities);

    /// <summary>
    /// Adds a collection of entities to the universe, initializing them.
    /// </summary>
    public void AddEntities(IEnumerable<TEntity> entities) {
        foreach (var entity in entities) AddEntity(entity);
    }

    /// <summary>
    /// Adds an entity to the universe, initializing it.
    /// </summary>
    public void AddEntity(TEntity entity) {
        if (entity.Universe != null) throw new InvalidOperationException("Entity already belongs to a universe.");

        entity.Initialize(this, _entityIndexPool.Borrow());
        _entities[entity.Index] = entity;
    }

    /// <summary>
    /// Removes entities from the universe.
    /// </summary>
    public void RemoveEntities(params TEntity[] entities) => RemoveEntities((IEnumerable<TEntity>)entities);

    /// <summary>
    /// Removes a collection of entities from the universe.
    /// </summary>
    public void RemoveEntities(IEnumerable<TEntity> entities) {
        foreach (var entity in entities) RemoveEntity(entity);
    }

    /// <summary>
    /// Removes an entity from the universe.
    /// </summary>
    public void RemoveEntity(TEntity entity) {
        if (entity.Universe != this)
            throw new InvalidOperationException("Entity does not belong to this universe.");

        _entityIndexPool.Free(entity.Index);
        entity.Uninitialize();
    }

    public PrefabBuilder<TEntity> MakePrefab() => new(this);

    /// <summary>
    /// Creates a prefab for the current state of the given entity (*not* the original prefab it was created with, if any).
    /// The prefab will have all the components this entity does now, and the default values for those components will be
    /// the current values for this entity.<br /><br />
    /// This operation can be expensive if the entity has a lot of components, so try to create your prefabs statically
    /// if possible.
    /// </summary>
    public Prefab<TEntity> MakePrefab(TEntity entity) {
        var bits = new BitArray(entity.ComponentBits);
        var defaults = new Dictionary<Type, object>();

        foreach (var (type, index) in _typeToIndex) {
            if (!(bits.Count > index && bits[index])) continue;

            var value = _componentBags[index].GetCopy(entity);
            if (value != null) defaults[type] = value;
        }

        return new Prefab<TEntity>(this, bits, new ReadOnlyDictionary<Type, object>(defaults));
    }

    /// <summary>
    /// Creates a prefab with the given components.
    /// </summary>
    /// <param name="componentTypes"></param>
    /// <returns></returns>
    public Prefab<TEntity> MakePrefab(params Type[] componentTypes) {
        var bits = GetComponentBits(componentTypes);
        return new Prefab<TEntity>(this, bits, null);
    }

    /// <summary>
    /// Creates a prefab with the given components as default values. Note that the components passed here must be
    /// structs; we just use object to avoid having to specify a type parameter for each component. I trust you.
    /// </summary>
    public Prefab<TEntity> MakePrefabFromDefaults(params object[] components) {
        var bits = GetComponentBits(from c in components select c.GetType());
        var defaults = components.ToDictionary(component => component.GetType());
        return new Prefab<TEntity>(this, bits, new ReadOnlyDictionary<Type, object>(defaults));
    }

    /// <summary>
    /// Gets a component accessor for the given component type. If the component type has not been used before, it will be
    /// registered. Keeping a reference to this accessor is the preferred way to manage components, to avoid extra lookups.
    /// </summary>
    public ComponentAccessor<TEntity, T> GetComponentAccessor<T>() where T : struct {
        if (!_typeToIndex.TryGetValue(typeof(T), out var index)) {
            index = _componentBags.Count;
            var bag = _componentBags[index] = new ComponentBag<TEntity, T>(this, index);
            _typeToIndex.Add(typeof(T), index);
            return (ComponentAccessor<TEntity, T>)bag;
        }

        return (ComponentAccessor<TEntity, T>)_componentBags[index];
    }

    /// <summary>
    /// Attaches a component to an entity. If the entity already has the component, it will be overwritten.<br /><br />
    /// If you are attaching the same component more than once, prefer keeping a reference to the accessor returned by
    /// <see cref="GetComponentAccessor{T}"/> and calling <see cref="ComponentAccessor{TEntity,TComponent}.Put(TEntity)"/> on it
    /// instead for performance.<br /><br />
    /// Returns a reference to the component that was attached.
    /// </summary>
    public ref TComponent AttachComponent<TComponent>(TEntity entity) where TComponent : struct {
        return ref AttachComponent(entity, new TComponent());
    }

    /// <summary>
    /// Attaches a component to an entity. If the entity already has the component, it will be overwritten.<br /><br />
    /// If you are attaching the same component more than once, prefer keeping a reference to the accessor returned by
    /// <see cref="GetComponentAccessor{T}"/> and calling <see cref="ComponentAccessor{TEntity,TComponent}.Put(TEntity, TComponent)"/>
    /// on it instead for performance.<br /><br />
    /// Returns a reference to the component that was attached.
    /// </summary>
    public ref T AttachComponent<T>(TEntity entity, T component) where T : struct {
        if (entity.Universe != this) throw new SpaceAlienException(nameof(entity));

        var accessor = GetComponentAccessor<T>();
        return ref accessor.Put(entity, component);
    }

    internal BitArray GetComponentBits(params Type[] types) {
        var bits = new BitArray(_componentBags.Count);
        foreach (var type in types) {
            if (!_typeToIndex.TryGetValue(type, out var index)) continue;
            bits.Set(index, true);
        }

        return bits;
    }

    internal BitArray GetComponentBits(IEnumerable<Type> types) {
        var bits = new BitArray(_componentBags.Count);
        foreach (var type in types) {
            if (!_typeToIndex.TryGetValue(type, out var index)) continue;
            bits.Set(index, true);
        }

        return bits;
    }

    internal int GetComponentTypeIndex<T>() where T : struct {
        if (!_typeToIndex.TryGetValue(typeof(T), out var index)) {
            throw new SpaceAlienException($"Component type {typeof(T)} has not been registered.");
        }

        return index;
    }
}