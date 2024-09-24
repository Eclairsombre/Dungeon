using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass;
using Dungeon.src.InterfaceClass;
using Dungeon.src;

namespace Dungeon;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;



    private Player player;
    private Map map;

    private static Rectangle sourceRectangle;


    private Interface gameInterface;

    //private Animation animation;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();

        //animation = new Animation("testRegression", MyCallback, 0, 0);
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

        //animation.LoadContent(Content);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //animation.Update(gameTime);

        player.Update(gameTime, map, Content);
        map.Update(player.CenterPosition, gameTime);
        base.Update(gameTime);
    }



    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        _spriteBatch.Begin();

        map.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        gameInterface.Draw(_spriteBatch, player);

        //float scale = animation.GetScale();
        //_spriteBatch.Draw(animation.texture, new Vector2(100, 100), sourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
