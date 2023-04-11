using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Screens;

namespace StarFoundry.Engine.Core;

/// <summary>
/// A container that events are delegated to from the game instance.
/// </summary>
public class Container : Screen {
    public static readonly Container Empty = new();

    public sealed override void LoadContent() {
        LoadContent(Client.ContentManager);
    }

    protected virtual void LoadContent(ContentManager content) { }

    public override void Update(GameTime gameTime) { }

    public override void Draw(GameTime gameTime) { }
}