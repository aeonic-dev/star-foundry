using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarFoundry.Engine.Core;

namespace StarFoundry.GameContent;

public class TestContainer : Container {
    private Vector2 _direction = new(1, 1);
    private Rectangle _target = new(0, 0, 150, 100);
    private Texture2D _texture = null!;

    protected override void LoadContent(ContentManager content) {
        _texture = content.Load<Texture2D>("textures/test");
    }

    public override void Update(GameTime gameTime) {
        if ((_target.Right >= Client.ScreenSize.Width && _direction.X > 0) || (_target.Left <= 0 && _direction.X < 0))
            _direction.X *= -1;
        if ((_target.Bottom >= Client.ScreenSize.Height && _direction.Y > 0) || (_target.Top <= 0 && _direction.Y < 0))
            _direction.Y *= -1;

        _target.Location += (_direction * gameTime.ElapsedGameTime.Milliseconds * .2f).ToPoint();
    }

    public override void Draw(GameTime gameTime) {
        Client.SpriteBatch.Begin();
        Client.SpriteBatch.Draw(_texture, _target, Color.White);
        Client.SpriteBatch.End();
    }
}