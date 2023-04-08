using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace StarFoundry.Engine;

/// <summary>
/// A container that events are delegated to from the game instance.
/// </summary>
public class Container : Screen {
    public static readonly Container Empty = new();

    public override void Update(GameTime gameTime) { }

    public override void Draw(GameTime gameTime) { }
}