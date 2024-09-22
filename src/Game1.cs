using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass;
using Dungeon.src.InterfaceClass;

namespace Dungeon.src;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;



    private Player player;
    private Map map;

    private Interface gameInterface;
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
        player = new Player(_graphics.GraphicsDevice);
        map = new Map();
        gameInterface = new Interface();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        player.LoadContent(Content);
        map.GenerateDungeon(Content);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        player.Update(gameTime, map, Content);
        map.Update(player.CenterPosition, gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        map.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        gameInterface.Draw(_spriteBatch, player);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
