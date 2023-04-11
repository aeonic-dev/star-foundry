using MonoGame.Extended.Input.InputListeners;

namespace StarFoundry.Engine.Input.Listeners;

public interface KeyboardEvents {
    public virtual void OnKeyTyped(object? sender, KeyboardEventArgs keyboard) { }

    public virtual void OnKeyPressed(object? sender, KeyboardEventArgs keyboard) { }

    public virtual void OnKeyReleased(object? sender, KeyboardEventArgs keyboard) { }
}