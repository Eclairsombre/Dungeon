using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Microsoft.Xna.Framework.Content;

namespace Dungeon.src.MapClass;


public class Map
{


    private Random random = new Random();

    public Room room;


    public int screenWidth;
    public int screenHeight;

    public Map()
    {
        screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

    }

    public void GenerateDungeon(ContentManager content)
    {
        room = new Room();
        room.LoadContent(content, 1);
        room.Generate();

    }

    public void Update(Vector2 playerPosition)
    {
        room.Update(playerPosition);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        room.Draw(spriteBatch);
    }


}