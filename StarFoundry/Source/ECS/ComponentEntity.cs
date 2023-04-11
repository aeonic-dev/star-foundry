using System;
using System.Collections;

namespace StarFoundry.ECS;

/// <summary>
/// An entity in the ECS. Not the same as an actual game entity; this interface is used both for them and for tile entities.
/// Hence the generic parameter.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public abstract class ComponentEntity<TSelf> where TSelf : ComponentEntity<TSelf> {
    /// <summary>
    /// The universe this entity belongs to.
    /// </summary>
    public Universe<TSelf> Universe { get; private set; } = null!;

    internal readonly Prefab<TSelf>? Prefab;
    internal int Index = -1; // might be faster to give each entity a separate index per type
    internal BitArray ComponentBits { get; private set; }

    /// <summary>
    /// Constructs a new entity. If a prefab is provided, the entity will be initialized with the components and default
    /// values from the prefab.
    /// </summary>
    protected ComponentEntity(Prefab<TSelf>? prefab = null) {
        Prefab = prefab;
        ComponentBits = prefab == null ? new BitArray(64) : new BitArray(prefab.ComponentBits);
    }

    internal void Initialize(Universe<TSelf> universe, int index) {
        if (Prefab != null && Prefab.Universe != universe)
            throw new ArgumentException("Prefab must belong to the same universe as the entity.", nameof(Prefab));

        Universe = universe;
        Index = index;
    }

    internal void Uninitialize() {
        Universe = null!;
        Index = -1;
    }

    /// <summary>
    /// Creates a new prefab from the current state of this entity (*not* the original prefab it was created with, if any).
    /// The prefab will have all the components this entity does now, and the default values for those components will be
    /// the current values for this entity.
    /// </summary>
    /// <returns></returns>
    public Prefab<TSelf> ToPrefab() {
        return Universe.MakePrefab((TSelf)this);
    }

    /// <summary>
    /// Attaches a component to an entity. If the entity already has the component, it will be overwritten.<br /><br />
    /// This method can only be called after the entity has been added to a universe, or else an exception will be thrown.
    /// Returns a reference to the component that was attached. See Universe#AttachComponent for more information.<br /><br />
    /// Prefer keeping a reference to the accessor returned by <see cref="StarFoundry.ECS.Universe.GetComponentAccessor"/>
    /// for performance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ref T Attach<T>() where T : struct {
        if (Universe == null)
            throw new InvalidOperationException(
                "Cannot attach a component to an entity that has not been added to a universe.");

        return ref Universe.AttachComponent<T>((TSelf)this);
    }

    /// <summary>
    /// Attaches a component to an entity. If the entity already has the component, it will be overwritten.<br /><br />
    /// This method can only be called after the entity has been added to a universe, or else an exception will be thrown.
    /// Returns a reference to the component that was attached. See Universe#AttachComponent for more information.<br /><br />
    /// Prefer keeping a reference to the accessor returned by <see cref="StarFoundry.ECS.Universe.GetComponentAccessor"/>
    /// for performance.
    /// </summary>
    public ref T Attach<T>(T component) where T : struct {
        if (Universe == null)
            throw new InvalidOperationException(
                "Cannot attach a component to an entity that has not been added to a universe.");

        return ref Universe.AttachComponent<>((TSelf)this, component);
    }

    /// <summary>
    /// Called when the entity is added to a universe. <see cref="Universe"/> is already set at this point.
    /// </summary>
    public virtual void OnInitialize() { }
}