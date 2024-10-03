using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dungeon.src.MenuClass;

namespace Dungeon.src;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;

    private Menu menu;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        /// _graphics.ToggleFullScreen();
        _graphics.ApplyChanges();

    }

    protected override void Initialize()
    {
        menu = new Menu(_graphics.GraphicsDevice, Content);
        menu.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        menu.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
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
