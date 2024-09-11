using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
namespace Dungeon;

public class Door
{
    public int idRoomToGo;
    public String direction;

    public Rectangle hitbox;
    public Door(int idRoomToGo, String direction, int screenWidth, int screenHeight)
    {
        this.idRoomToGo = idRoomToGo;
        this.direction = direction;

        switch (direction)
        {
            case "up":
                hitbox = new Rectangle(screenWidth * 35 / 2, 40, 10, 10);
                break;
            case "down":
                hitbox = new Rectangle(screenWidth * 35 / 2, screenHeight * 32 + 50, 10, 10);
                break;
            case "left":
                hitbox = new Rectangle(40, screenHeight * 32 / 2 + 10, 10, 10);
                break;
            case "right":
                hitbox = new Rectangle(screenWidth * 35 + 30, screenHeight * 32 / 2 + 10, 10, 10);
                break;
        }

    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(hitbox, Color.Red);
    }

    public void printHitbox()
    {
        Console.WriteLine(hitbox);
    }
}