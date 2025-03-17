using System;
using System.Linq;
using Dungeon.src.DropClass;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Dungeon.src.MapClass.HolderClass;
using Dungeon.src.PlayerClass.WeaponClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dungeon.src.CollisionClass;
using Dungeon.src.PlayerClass.StatsClass;
using Dungeon.src.MenuClass;
using MonoGame.Extended;
using Dungeon.src.InterfaceClass;

namespace Dungeon.src.PlayerClass
{
    public class Player
    {
        private const float DefaultScale = 3.0f;
        private const float DefaultSpeed = 10.0f;
        private const int Deadzone = 4096;
        private Vector2 position, direction, directionDeplacement;
        private readonly float speed;
        private float scale;
        private readonly Texture2D hitboxTexture;
        private int spriteWidth, spriteHeight;
        private Rectangle hitbox;
        private Stats stats = new();
        public Stats PlayerStats { get { return stats; } set { stats = value; } }
        private int invincibilityTime = 0;
        private const float useCooldown = 500f, dashCooldown = 250f;
        private float useCooldownTimer = 0, dashCooldownTimer = 0;
        private bool isMovingHorizontally = false;
        private bool isMovingVertically = false;

        private bool dash = false;
        public int SpriteWidth { get { return spriteWidth; } set { spriteWidth = value; } }
        public int SpriteHeight { get { return spriteHeight; } set { spriteHeight = value; } }
        public int Scale { get { return (int)scale; } set { scale = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Direction { get { return direction; } set { direction = value; } }

        public Player(GraphicsDevice graphicsDevice)
        {
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 40;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 40;
            position = new Vector2(screenWidth / 2 - 65, screenHeight - 500);
            speed = DefaultSpeed;
            scale = DefaultScale;
            hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });
            direction = new Vector2(0, -1);
            directionDeplacement = new Vector2(0, 0);
            hitbox = new Rectangle((int)position.X + 5, (int)position.Y + 5, (int)(spriteWidth * scale) - 5, (int)(spriteHeight * scale) - 10);
        }

        public void LoadContent(ContentManager content)
        {
            spriteWidth = 35;
            spriteHeight = 35;
        }


        public void PlayerKeyboardAndMouseInput(GameTime gameTime, KeyBind keyBind)
        {
            var keyboardState = Keyboard.GetState();

            UpdateMovement(keyboardState, keyBind.keyBindings["Up"][0], keyBind.keyBindings["Up"].Length > 1 ? keyBind.keyBindings["Up"][1] : Keys.None, new Vector2(0, -1));
            UpdateMovement(keyboardState, keyBind.keyBindings["Down"][0], keyBind.keyBindings["Down"].Length > 1 ? keyBind.keyBindings["Down"][1] : Keys.None, new Vector2(0, 1));
            UpdateMovement(keyboardState, keyBind.keyBindings["Left"][0], keyBind.keyBindings["Left"].Length > 1 ? keyBind.keyBindings["Left"][1] : Keys.None, new Vector2(-1, 0));
            UpdateMovement(keyboardState, keyBind.keyBindings["Right"][0], keyBind.keyBindings["Right"].Length > 1 ? keyBind.keyBindings["Right"][1] : Keys.None, new Vector2(1, 0));

            if (keyboardState.IsKeyDown(keyBind.keyBindings["Dash"][0]) || (keyBind.keyBindings["Dash"].Length > 1 && keyboardState.IsKeyDown(keyBind.keyBindings["Dash"][1])))
            {
                if (dashCooldownTimer >= dashCooldown)
                {
                    dashCooldownTimer = 0;
                    dash = true;
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space))
            {
                dashCooldownTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                dash = false;
            }
        }

        private void UpdateMovement(KeyboardState keyboardState, Keys primaryKey, Keys secondaryKey, Vector2 directionVector)
        {
            if (keyboardState.IsKeyDown(primaryKey) || keyboardState.IsKeyDown(secondaryKey))
            {
                if (directionVector.X != 0) isMovingHorizontally = true;
                if (directionVector.Y != 0) isMovingVertically = true;
                directionDeplacement += directionVector;
                direction = directionVector;
            }
            else if (keyboardState.IsKeyUp(primaryKey) && keyboardState.IsKeyUp(secondaryKey))
            {
                if (directionVector.X != 0) isMovingHorizontally = false;
                if (directionVector.Y != 0) isMovingVertically = false;
            }
        }



        public void PlayerCollision(Map map, ContentManager content, Rectangle newHitbox, Vector2 futurePosition, GameTime gameTime, ref GameState gameState)
        {

        }
        public void Update(GameTime gameTime, Map map, ContentManager content, ref GameState gameState, KeyBind keyBind, ref Camera camera)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invincibilityTime > 0)
            {
                invincibilityTime -= (int)(deltaTime * 1000);
            }

            Vector2 futurePosition = position;
            directionDeplacement = new Vector2(0, 0);
            PlayerKeyboardAndMouseInput(gameTime, keyBind);
            JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);

            if (jstate.IsConnected)
            {
                if (jstate.Axes[1] < -Deadzone)
                {
                    position.Y -= speed;
                }
                else if (jstate.Axes[1] > Deadzone)
                {
                    position.Y += speed;
                }

                if (jstate.Axes[0] < -Deadzone)
                {
                    position.X -= speed;
                }
                else if (jstate.Axes[0] > Deadzone)
                {
                    position.X += speed;
                }
            }

            Deplacement(ref futurePosition);
            Rectangle newHitbox = new((int)futurePosition.X + 10, (int)futurePosition.Y + 10, (int)(spriteWidth * scale) - 10, (int)(spriteHeight * scale) - 15);
            PlayerCollision(map, content, newHitbox, futurePosition, gameTime, ref gameState);
            position = futurePosition;
            hitbox = newHitbox;

            Rectangle baseFloor = new(160, 120, 1600, 800);
            if (!baseFloor.Contains(hitbox))
            {
                camera.Update(position);
            }
            else
            {
                camera.Reset();
            }

        }

        public void Deplacement(ref Vector2 futurePosition)
        {

            float valAbs = (float)Math.Sqrt(directionDeplacement.X * directionDeplacement.X + directionDeplacement.Y * directionDeplacement.Y);
            if (valAbs == 0)
            {
                return;
            }
            directionDeplacement = new Vector2(directionDeplacement.X / (float)Math.Sqrt(valAbs), directionDeplacement.Y / (float)Math.Sqrt(valAbs));
            float adjustedSpeed = (isMovingHorizontally && isMovingVertically) ? speed * stats.Speed / (float)Math.Sqrt(2) : speed * stats.Speed;
            if (dash)
            {
                adjustedSpeed *= 2;
            }
            futurePosition = new Vector2(position.X + directionDeplacement.X * adjustedSpeed, position.Y + (directionDeplacement.Y * adjustedSpeed));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawRectangle(spriteBatch, hitbox, Color.Red);
        }
        private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.FillRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), color);
        }
    }
}