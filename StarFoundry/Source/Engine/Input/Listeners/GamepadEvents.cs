using MonoGame.Extended.Input.InputListeners;

namespace StarFoundry.Engine.Input.Listeners;

public interface GamepadEvents {
    public virtual void OnGamepadButtonDown(object? sender, GamePadEventArgs gamepad) { }

    public virtual void OnGamepadButtonUp(object? sender, GamePadEventArgs gamepad) { }

    public virtual void OnGamepadButtonRepeated(object? sender, GamePadEventArgs gamepad) { }

    public virtual void OnGamepadThumbstickMoved(object? sender, GamePadEventArgs gamepad) { }

    public virtual void OnGamepadTriggerMoved(object? sender, GamePadEventArgs gamepad) { }
}