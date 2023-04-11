using System;
using System.Collections.Generic;
using System.Reflection;
using MonoGame.Extended.Collections;
using StarFoundry.Misc;

namespace StarFoundry.Engine.ECS;

public interface IComponentBag {
    internal object? GetCopy(object entity);
}

public class ComponentBag<TEntity, TComponent> : Bag<TComponent>, IComponentBag, ComponentAccessor<TEntity, TComponent>
    where TEntity : ComponentEntity<TEntity> where TComponent : struct {
    private static readonly FieldInfo ItemsField =
        typeof(Bag<TComponent>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)!;

    public Universe<TEntity> Universe { get; }

    internal readonly int Index;

    private TComponent[] _items;
    private readonly HashSet<int> _entityIndices = new();

    public ComponentBag(Universe<TEntity> universe, int index) {
        Universe = universe;
        Index = index;

        _items = (TComponent[])ItemsField.GetValue(this)!;
    }

    public ref TComponent GetByReference(TEntity entity) {
        if (!Has(entity)) throw new InvalidOperationException("Entity does not have this component.");

        // If the entity has been added to the bag, it will have an index. If it hasn't, it will be initialized with a
        // default value either from a prefab or a new instance.
        if (!_entityIndices.Contains(entity.Index)) {
            EnsureCapacity(entity.Index + 1);
            if (entity.Prefab != null) _items[entity.Index] = entity.Prefab.GetDefaultValue<TComponent>();
            else _items[entity.Index] = new TComponent();

            _entityIndices.Add(entity.Index);
        }

        return ref _items[entity.Index];
    }

    int ComponentAccessor<TEntity, TComponent>.ComponentTypeIndex => Index;

    public bool Has(TEntity entity) {
        return entity.ComponentBits.Length > Index && entity.ComponentBits[Index];
    }

    public ref TComponent Get(TEntity entity) {
        return ref GetByReference(entity);
    }

    public ref TComponent Put(TEntity entity) {
        return ref Put(entity, new TComponent());
    }

    public ref TComponent Put(TEntity entity, TComponent component) {
        EnsureCapacity(entity.Index + 1);
        entity.ComponentBits.EnsureLength(Index + 1);

        _items[entity.Index] = component;
        entity.ComponentBits[Index] = true;
        _entityIndices.Add(entity.Index);

        return ref _items[entity.Index];
    }

    public void Remove(TEntity entity) {
        if (!Has(entity)) return;

        entity.ComponentBits.EnsureLength(Index + 1);
        entity.ComponentBits[Index] = false;

        EnsureCapacity(entity.Index);
        _items[entity.Index] = default;
        _entityIndices.Remove(entity.Index);
    }

    public ref TComponent this[TEntity entity] => ref Get(entity);

    object? IComponentBag.GetCopy(object entity) {
        if (entity is not TEntity typedEntity) throw new ArgumentException(null, nameof(entity));
        if (Has(typedEntity)) return Get(typedEntity);
        return null;
    }

    private void EnsureCapacity(int capacity) {
        if (capacity < _items.Length) return;

        var length = Math.Max((int)(_items.Length * 1.5), capacity);
        var items = _items;
        _items = new TComponent[length];

        Array.Copy(items, 0, _items, 0, items.Length);
    }
}