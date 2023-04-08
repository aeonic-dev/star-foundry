namespace StarFoundry.Engine;

/// <summary>
/// Tracks the state of the resize operation, used so we can keep drawing while the resize is pending.
/// Inspired by https://community.monogame.net/t/handling-user-controlled-window-resizing/7828
/// </summary>
public readonly struct ResizeState {
    public readonly bool ResizePending;
    public readonly int Width;
    public readonly int Height;

    public ResizeState(bool resizePending, int width, int height) {
        ResizePending = resizePending;
        Width = width;
        Height = height;
    }
}