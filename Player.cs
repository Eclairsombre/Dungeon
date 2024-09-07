using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon;

public class Player
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Speed;

    private Texture2D spriteSheet;
    private int spriteWidth;
    private int spriteHeight;
    private int currentSpriteIndexX, currentSpriteIndexY;
    private float scale = 3;

    public int deadzone = 4096;
    private Texture2D hitboxTexture;

    public SpriteEffects spriteEffect = SpriteEffects.None;
    public Player(Texture2D spriteSheet, int spriteWidth, int spriteHeight, GraphicsDevice graphicsDevice)
    {

        this.spriteSheet = spriteSheet;
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        Position = new Vector2(100, 100);
        Speed = 5;
        currentSpriteIndexX = 0;
        currentSpriteIndexY = 0;

        spriteSheet.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

        hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
        hitboxTexture.SetData(new[] { Color.White });


    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            Position.Y -= Speed;
            currentSpriteIndexX = 2;
            spriteEffect = SpriteEffects.None;

        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            Position.Y += Speed;
            currentSpriteIndexX = 0;
            spriteEffect = SpriteEffects.None;

        }
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            Position.X -= Speed;
            currentSpriteIndexX = 1;
            spriteEffect = SpriteEffects.None;
        }
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            Position.X += Speed;
            currentSpriteIndexX = 1;
            spriteEffect = SpriteEffects.FlipHorizontally;
        }
        JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);

        float updatedPlayerSpeed = Speed;

        if (jstate.Axes[1] < -deadzone)
        {
            Position.Y -= updatedPlayerSpeed;
        }
        else if (jstate.Axes[1] > deadzone)
        {
            Position.Y += updatedPlayerSpeed;
        }

        if (jstate.Axes[0] < -deadzone)
        {
            Position.X -= updatedPlayerSpeed;
        }
        else if (jstate.Axes[0] > deadzone)
        {
            Position.X += updatedPlayerSpeed;
        }
    }


    public void SetSpriteIndex(int indexX, int indexY)
    {
        currentSpriteIndexX = indexX;
        currentSpriteIndexY = indexY;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRectangle = new Rectangle(currentSpriteIndexX * spriteWidth + 12 * (currentSpriteIndexX + 1), currentSpriteIndexY * spriteHeight + 5 * (currentSpriteIndexY + 1), spriteWidth, spriteHeight);
        spriteBatch.Draw(spriteSheet, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0f);

        Rectangle hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)(spriteWidth * scale), (int)(spriteHeight * scale));
        DrawRectangle(spriteBatch, hitbox, Color.Red);
    }

    private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        // Dessiner les quatre côtés du rectangle
        spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color); // Haut
        spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color); // Bas
        spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color); // Gauche
        spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height), color); // Droite
    }
}