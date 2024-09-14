using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

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

    public void GenerateDungeon()
    {
        room = new Room();

    }






    public void Draw(SpriteBatch spriteBatch)
    {
        room.Draw(spriteBatch);
    }


}