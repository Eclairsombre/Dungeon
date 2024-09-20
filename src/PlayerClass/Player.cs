using System;
using System.Linq;
using Dungeon.src.DropClass;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Dungeon.src.PlayerClass
{
    public class Player
    {
        private const float DefaultScale = 3.0f;
        private const float DefaultSpeed = 10.0f;
        private const int Deadzone = 4096;

        public Vector2 Position;
        public Vector2 centerPosition;

        public Vector2 StartPosition;
        public float Speed;

        private Texture2D[] spriteSheetNoMove = new Texture2D[3];
        private int currentSpriteSheet = 0;
        public int spriteWidth;
        public int spriteHeight;

        public float scale;

        public Rectangle hitbox;

        private Texture2D hitboxTexture;

        public SpriteEffects spriteEffect = SpriteEffects.None;

        public Vector2 direction;

        public Weapon weapon;
        public Vector2 weaponPosition;

        public bool attack;

        public int xp = 0;
        public int level = 1;

        public int nbHeart = 3;


        public int xpToLevelUp = 100;

        public int invincibilityTime = 0;


        public Player(GraphicsDevice graphicsDevice)
        {
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 40;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 40;
            Position = new Vector2(screenWidth / 2 - 65, screenHeight - 200);
            StartPosition = new Vector2(screenWidth / 2 - 65, screenHeight - 200);
            Speed = DefaultSpeed;
            scale = DefaultScale;

            centerPosition = new Vector2((Position.X + spriteWidth) * scale / 2, (Position.Y + spriteHeight) * scale / 2);


            hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });

            this.direction = new Vector2(0, 1);

            weapon = new Sword(Position);
            Console.WriteLine(weapon.range);

            attack = false;
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheetNoMove[0] = content.Load<Texture2D>("PlayerNoMoveFront");
            spriteSheetNoMove[1] = content.Load<Texture2D>("PlayerNoMoveLeft");
            spriteSheetNoMove[2] = content.Load<Texture2D>("PlayerNoMoveBack");
            spriteWidth = spriteSheetNoMove[0].Width;
            spriteHeight = spriteSheetNoMove[0].Height;

            // Appliquer un filtre de texture pour améliorer la qualité
            foreach (var texture in spriteSheetNoMove)
            {
                texture.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            }
        }



        public bool CheckCollisionWithRoom(Room room)
        {
            for (int i = 0; i < room.tiles.GetLength(0); i++)
            {
                for (int y = 0; y < room.tiles.GetLength(1); y++)
                {
                    if (room.tiles[i, y].id.Item1 == 1)
                    {

                        if (CheckCollision(room.tiles[i, y].hitbox))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Equip(Weapon weapon)
        {
            this.weapon = weapon;
        }

        public bool CheckCollision(Rectangle rect)
        {
            Rectangle playerHitbox = GetHitbox();

            if (playerHitbox.Intersects(rect))
            {
                return true;
            }

            return false;
        }

        public bool CheckCollisionWithDoor(Room room)
        {
            if (room.finished)
            {
                Rectangle playerHitbox = GetHitbox();
                Door door1 = room.tiles[10, 0].door;
                Door door2 = room.tiles[14, 0].door;
                if (door1 != null)
                {
                    if (playerHitbox.Intersects(door1.hitbox))
                    {
                        return true;
                    }
                }
                if (door2 != null)
                {
                    if (playerHitbox.Intersects(door2.hitbox))
                    {
                        return true;
                    }
                }

            }


            return false;
        }


        public void Update(GameTime gameTime, Map map, int screenWidth, int screenHeight, ContentManager content, SpriteBatch spriteBatch)
        {
            var keyboardState = Keyboard.GetState();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (invincibilityTime > 0)
            {
                invincibilityTime -= (int)(deltaTime * 1000);
            }

            Rectangle roomHitbox = new Rectangle(40, 40, 52 * 35, 30 * 32 + 20);

            Vector2 previousPosition = Position;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y -= Speed;
                currentSpriteSheet = 2;
                spriteEffect = SpriteEffects.None;
                this.direction = new Vector2(0, 1);
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                Position.Y += Speed;
                currentSpriteSheet = 0;
                spriteEffect = SpriteEffects.None;
                this.direction = new Vector2(0, -1);
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                Position.X -= Speed;
                currentSpriteSheet = 1;
                spriteEffect = SpriteEffects.None;
                this.direction = new Vector2(-1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                Position.X += Speed;
                currentSpriteSheet = 1;
                spriteEffect = SpriteEffects.FlipHorizontally;
                this.direction = new Vector2(1, 0);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                attack = true;
                weapon.Update(this, direction, map.room.enemies);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                attack = false;
            }

            // Gestion du joystick
            JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);
            float updatedPlayerSpeed = Speed;

            if (jstate.IsConnected)
            {
                if (jstate.Axes[1] < -Deadzone)
                {
                    Position.Y -= updatedPlayerSpeed;
                }
                else if (jstate.Axes[1] > Deadzone)
                {
                    Position.Y += updatedPlayerSpeed;
                }

                if (jstate.Axes[0] < -Deadzone)
                {
                    Position.X -= updatedPlayerSpeed;
                }
                else if (jstate.Axes[0] > Deadzone)
                {
                    Position.X += updatedPlayerSpeed;
                }
            }

            if (CheckCollisionWithDoor(map.room))
            {
                map.room = new Room();
                Random random = new Random();
                int roomNumber = random.Next(2, 5);
                Console.WriteLine(roomNumber);
                map.room.LoadContent(content, roomNumber);
                map.room.Generate();
                Position = StartPosition;
            }

            if (CheckCollisionWithRoom(map.room))
            {
                Position = previousPosition;
            }

            for (int i = 0; i < map.room.dropsList.Length; i++)
            {

                if (map.room.dropsList[i].IsColliding(this.hitbox))
                {
                    if (map.room.dropsList[i] is XpDrop xpDrop)
                    {
                        xp += xpDrop.xp;
                        if (xp >= xpToLevelUp)
                        {
                            xp -= xpToLevelUp;
                            level++;
                            xpToLevelUp = (int)(xpToLevelUp * 1.2);
                        }
                    }
                    map.room.dropsList = map.room.dropsList.Where((drop, index) => index != i).ToArray();
                }
            }
            if (invincibilityTime <= 0)
            {
                foreach (Enemy enemy in map.room.enemies)
                {
                    if (CheckCollision(enemy.hitbox))
                    {
                        nbHeart--;
                        invincibilityTime = 3000;
                        break;
                    }
                }
            }

            this.centerPosition = new Vector2(Position.X + spriteWidth * scale / 2, Position.Y + spriteHeight * scale / 2);
            weapon.Update(this, direction, map.room.enemies);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
            spriteBatch.Draw(spriteSheetNoMove[currentSpriteSheet], Position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0f);

            hitbox = new Rectangle((int)Position.X + 5, (int)Position.Y + 5, (int)(spriteWidth * scale) - 5, (int)(spriteHeight * scale) - 10);
            DrawRectangle(spriteBatch, hitbox, Color.Red);
            if (attack)
            {
                weapon.Draw(spriteBatch);

            }




        }

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)(Position.X) + 5, (int)Position.Y + 5, (int)(spriteWidth * scale) - 5, (int)(spriteHeight * scale) - 10);
        }

        private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color); // Haut
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color); // Bas
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color); // Gauche
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height), color); // Droite
        }
    }
}