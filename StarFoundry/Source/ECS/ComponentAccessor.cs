namespace StarFoundry.ECS;

/// <summary>
/// Provides access to all components of a given type for any entity in the same universe this accessor belongs to.
/// </summary>
public interface ComponentAccessor<TEntity, TComponent>
    where TEntity : ComponentEntity<TEntity> where TComponent : struct {
    public Universe<TEntity> Universe { get; }

    internal int ComponentTypeIndex { get; }

    /// <summary>
    /// Checks whether the given entity has a component of this type.
    /// </summary>
    public bool Has(TEntity entity);

    /// <summary>
    /// Gets a copy of the component of this type for the given entity, or null if one doesn't exist.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TComponent? GetCopy(TEntity entity) {
        if (TryGetCopy(entity, out var component)) return component;
        return null;
    }

    /// <summary>
    /// Sets the provided reference to a copy of the component of this type for the given entity, if one exists.
    /// If none exists, set the reference to the default value. Returns true if a component was found, false otherwise.
    /// </summary>
    public bool TryGetCopy(TEntity entity, out TComponent component) {
        if (entity.Universe != Universe) throw new SpaceAlienException(nameof(entity));

        if (Has(entity)) {
            component = Get(entity);
            return true;
        }

        component = default;
        return false;
    }

    /// <summary>
    /// Gets the component of this type for the given entity, or throws an error if one doesn't exist.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public ref TComponent Get(TEntity entity);

    /// <summary>
    /// Puts a component of this type (with default values) on the given entity. If one already exists, it will be overwritten.
    /// </summary>
    public ref TComponent Put(TEntity entity);

    /// <summary>
    /// Puts a component of this type on the given entity. If one already exists, it will be overwritten.
    /// </summary>
    public ref TComponent Put(TEntity entity, TComponent component);

    /// <summary>
    /// Removes the component of this type from the given entity, if it exists.
    /// </summary>
    public void Remove(TEntity entity);

    /// <summary>
    /// Gets the component of this type for the given entity, or throws an error if one doesn't exist.
    /// </summary>
    public ref TComponent this[TEntity entity] { get; }
}