using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using StarFoundry.Input.Listeners;

namespace StarFoundry.Input;

public class InputEvents : InputListenerComponent {
    public static InputEvents Instance { get; private set; } = null!;

    private static readonly TouchListener TouchListener = new();
    private static readonly GamePadListener GamePadListener = new();
    private static readonly KeyboardListener KeyboardListener = new();
    private static readonly MouseListener MouseListener = new();

    private InputEvents(Game game) : base(game, TouchListener, GamePadListener, KeyboardListener, MouseListener) { }

    /// <summary>
    /// Catch-all method for registering input events, registers any events that the listener implements.
    /// </summary>
    public static void RegisterInputEvents(object? listener) {
        switch (listener) {
            case TouchEvents touchEvents:
                RegisterTouchEvents(touchEvents);
                break;
            case GamepadEvents gamepadEvents:
                RegisterGamepadEvents(gamepadEvents);
                break;
            case KeyboardEvents keyboardEvents:
                RegisterKeyboardEvents(keyboardEvents);
                break;
            case MouseEvents mouseEvents:
                RegisterMouseEvents(mouseEvents);
                break;
        }
    }

    /// <summary>
    /// Catch-all method for unregistering input events, unregisters any events that the listener implements.
    /// </summary>
    public static void UnregisterInputEvents(object? listener) {
        switch (listener) {
            case TouchEvents touchEvents:
                UnregisterTouchEvents(touchEvents);
                break;
            case GamepadEvents gamepadEvents:
                UnregisterGamepadEvents(gamepadEvents);
                break;
            case KeyboardEvents keyboardEvents:
                UnregisterKeyboardEvents(keyboardEvents);
                break;
            case MouseEvents mouseEvents:
                UnregisterMouseEvents(mouseEvents);
                break;
        }
    }

    public static void RegisterTouchEvents(TouchEvents events) {
        TouchListener.TouchStarted += events.OnTouchStarted;
        TouchListener.TouchEnded += events.OnTouchEnded;
        TouchListener.TouchMoved += events.OnTouchMoved;
        TouchListener.TouchCancelled += events.OnTouchCancelled;
    }

    public static void UnregisterTouchEvents(TouchEvents events) {
        TouchListener.TouchStarted -= events.OnTouchStarted;
        TouchListener.TouchEnded -= events.OnTouchEnded;
        TouchListener.TouchMoved -= events.OnTouchMoved;
        TouchListener.TouchCancelled -= events.OnTouchCancelled;
    }

    public static void RegisterGamepadEvents(GamepadEvents events) {
        GamePadListener.ButtonDown += events.OnGamepadButtonDown;
        GamePadListener.ButtonUp += events.OnGamepadButtonUp;
        GamePadListener.ButtonRepeated += events.OnGamepadButtonRepeated;
        GamePadListener.ThumbStickMoved += events.OnGamepadThumbstickMoved;
        GamePadListener.TriggerMoved += events.OnGamepadTriggerMoved;
    }

    public static void UnregisterGamepadEvents(GamepadEvents events) {
        GamePadListener.ButtonDown -= events.OnGamepadButtonDown;
        GamePadListener.ButtonUp -= events.OnGamepadButtonUp;
        GamePadListener.ButtonRepeated -= events.OnGamepadButtonRepeated;
        GamePadListener.ThumbStickMoved -= events.OnGamepadThumbstickMoved;
        GamePadListener.TriggerMoved -= events.OnGamepadTriggerMoved;
    }

    public static void RegisterMouseEvents(MouseEvents events) {
        MouseListener.MouseDown += events.OnMouseDown;
        MouseListener.MouseUp += events.OnMouseUp;
        MouseListener.MouseClicked += events.OnMouseClicked;
        MouseListener.MouseDoubleClicked += events.OnMouseDoubleClicked;
        MouseListener.MouseMoved += events.OnMouseMoved;
        MouseListener.MouseWheelMoved += events.OnMouseWheelMoved;
        MouseListener.MouseDragStart += events.OnMouseDragStart;
        MouseListener.MouseDrag += events.OnMouseDrag;
        MouseListener.MouseDragEnd += events.OnMouseDragEnd;
    }

    public static void UnregisterMouseEvents(MouseEvents events) {
        MouseListener.MouseDown -= events.OnMouseDown;
        MouseListener.MouseUp -= events.OnMouseUp;
        MouseListener.MouseClicked -= events.OnMouseClicked;
        MouseListener.MouseDoubleClicked -= events.OnMouseDoubleClicked;
        MouseListener.MouseMoved -= events.OnMouseMoved;
        MouseListener.MouseWheelMoved -= events.OnMouseWheelMoved;
        MouseListener.MouseDragStart -= events.OnMouseDragStart;
        MouseListener.MouseDrag -= events.OnMouseDrag;
        MouseListener.MouseDragEnd -= events.OnMouseDragEnd;
    }

    public static void RegisterKeyboardEvents(KeyboardEvents events) {
        KeyboardListener.KeyPressed += events.OnKeyPressed;
        KeyboardListener.KeyReleased += events.OnKeyReleased;
        KeyboardListener.KeyTyped += events.OnKeyTyped;
    }

    public static void UnregisterKeyboardEvents(KeyboardEvents events) {
        KeyboardListener.KeyPressed -= events.OnKeyPressed;
        KeyboardListener.KeyReleased -= events.OnKeyReleased;
        KeyboardListener.KeyTyped -= events.OnKeyTyped;
    }

    public static void Bootstrap(Game game) {
        Instance = new InputEvents(game);
    }
}