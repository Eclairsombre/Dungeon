using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass;
using Dungeon.src.InterfaceClass;
using Dungeon.src;
using Dungeon.src.MenuClass;

namespace Dungeon;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Menu menu;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        menu = new Menu(_graphics.GraphicsDevice, _spriteBatch, Content);
        menu.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        menu.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        menu.Update(gameTime);
        base.Update(gameTime);
    }



    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        menu.Draw();



        base.Draw(gameTime);
    }
}
