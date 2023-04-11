using MonoGame.Extended.Input.InputListeners;

namespace StarFoundry.Engine.Input.Listeners;

public interface TouchEvents {
    public virtual void OnTouchStarted(object? sender, TouchEventArgs touch) { }

    public virtual void OnTouchEnded(object? sender, TouchEventArgs touch) { }

    public virtual void OnTouchMoved(object? sender, TouchEventArgs touch) { }

    public virtual void OnTouchCancelled(object? sender, TouchEventArgs touch) { }
}