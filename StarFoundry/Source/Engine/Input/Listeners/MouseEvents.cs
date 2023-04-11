using MonoGame.Extended.Input.InputListeners;

namespace StarFoundry.Engine.Input.Listeners;

public interface MouseEvents {
    public virtual void OnMouseDown(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseUp(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseClicked(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseDoubleClicked(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseMoved(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseWheelMoved(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseDragStart(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseDrag(object? sender, MouseEventArgs mouse) { }

    public virtual void OnMouseDragEnd(object? sender, MouseEventArgs mouse) { }
}