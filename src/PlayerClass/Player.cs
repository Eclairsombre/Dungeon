using System;
using System.Linq;
using Dungeon.src.DropClass;
using Dungeon.src.EnemyClass;
using Dungeon.src.MapClass;
using Dungeon.src.PlayerClass.WeaponClass;
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

        private Vector2 position, centerPosition, startPosition, direction;
        private float speed, scale;

        private readonly Texture2D[] spriteSheetNoMove = new Texture2D[3];
        private readonly Texture2D hitboxTexture;
        private int currentSpriteSheet;
        private SpriteEffects spriteEffect = SpriteEffects.None;

        private int spriteWidth, spriteHeight;
        private Rectangle hitbox;

        private Weapon weapon;

        private int level = 1, xp = 0;

        private int nbHeart = 3, xpToLevelUp = 100, invincibilityTime = 0;



        public int Level { get { return level; } set { level = value; } }
        public int Xp { get { return xp; } set { xp = value; } }
        public int NbHeart { get { return nbHeart; } set { nbHeart = value; } }
        public int XpToLevelUp { get { return xpToLevelUp; } set { xpToLevelUp = value; } }

        public int SpriteWidth { get { return spriteWidth; } set { spriteWidth = value; } }
        public int SpriteHeight { get { return spriteHeight; } set { spriteHeight = value; } }
        public int Scale { get { return (int)scale; } set { scale = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 CenterPosition { get { return centerPosition; } set { centerPosition = value; } }




        public Player(GraphicsDevice graphicsDevice)
        {
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 40;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 40;
            position = new Vector2(screenWidth / 2 - 65, screenHeight - 200);
            startPosition = new Vector2(screenWidth / 2 - 65, screenHeight - 200);
            speed = DefaultSpeed;
            scale = DefaultScale;
            centerPosition = new Vector2((position.X + spriteWidth) * scale / 2, (position.Y + spriteHeight) * scale / 2);
            hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });
            direction = new Vector2(0, 1);
            weapon = new Bow(centerPosition);
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheetNoMove[0] = content.Load<Texture2D>("PlayerNoMoveFront");
            spriteSheetNoMove[1] = content.Load<Texture2D>("PlayerNoMoveLeft");
            spriteSheetNoMove[2] = content.Load<Texture2D>("PlayerNoMoveBack");
            spriteWidth = spriteSheetNoMove[0].Width;
            spriteHeight = spriteSheetNoMove[0].Height;
            foreach (var texture in spriteSheetNoMove)
            {
                texture.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            }
            if (weapon is Bow bow)
            {
                bow.LoadContent(content);
            }

        }



        public bool CheckCollisionWithRoom(Room room)
        {
            for (int i = 0; i < room.Tiles.GetLength(0); i++)
            {
                for (int y = 0; y < room.Tiles.GetLength(1); y++)
                {
                    if (room.Tiles[i, y].Id.Item1 == 1)
                    {

                        if (CheckCollision(room.Tiles[i, y].Hitbox))
                        {
                            return true;
                        }
                    }
                    else if (room.Tiles[i, y].Id.Item1 == 4 && room.Finished)
                    {
                        if (CheckCollision(room.Tiles[i, y].Holder.Hitbox))
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
            if (room.Finished)
            {
                Rectangle playerHitbox = GetHitbox();
                Door door1 = room.Tiles[10, 0].Door;
                Door door2 = room.Tiles[14, 0].Door;
                if (door1 != null)
                {
                    if (playerHitbox.Intersects(door1.Hitbox))
                    {
                        return true;
                    }
                }
                if (door2 != null)
                {
                    if (playerHitbox.Intersects(door2.Hitbox))
                    {
                        return true;
                    }
                }

            }
            return false;
        }


        public void Update(GameTime gameTime, Map map, ContentManager content)
        {
            var keyboardState = Keyboard.GetState();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (invincibilityTime > 0)
            {
                invincibilityTime -= (int)(deltaTime * 1000);
            }

            Rectangle roomHitbox = new(40, 40, 52 * 35, 30 * 32 + 20);

            Vector2 previousPosition = position;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                position.Y -= speed;
                currentSpriteSheet = 2;
                spriteEffect = SpriteEffects.None;
                direction = new Vector2(0, 1);
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                position.Y += speed;
                currentSpriteSheet = 0;
                spriteEffect = SpriteEffects.None;
                direction = new Vector2(0, -1);
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                position.X -= speed;
                currentSpriteSheet = 1;
                spriteEffect = SpriteEffects.None;
                direction = new Vector2(-1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                position.X += speed;
                currentSpriteSheet = 1;
                spriteEffect = SpriteEffects.FlipHorizontally;
                direction = new Vector2(1, 0);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (weapon is Bow b)
                {
                    b.Attack(direction, centerPosition, content);
                }
                else
                {
                    weapon.Update(this, direction, map.ActualRoom.Enemies, gameTime);
                }
            }


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

            if (CheckCollisionWithDoor(map.ActualRoom))
            {
                map.ActualRoom = new Room();
                Random random = new();
                int roomNumber = random.Next(2, 5);
                map.ActualRoom.LoadContent(content, roomNumber);
                map.ActualRoom.Generate();
                position = startPosition;
            }

            if (CheckCollisionWithRoom(map.ActualRoom))
            {
                position = previousPosition;
            }

            for (int i = 0; i < map.ActualRoom.DropsList.Length; i++)
            {

                if (map.ActualRoom.DropsList[i].IsColliding(hitbox))
                {
                    if (map.ActualRoom.DropsList[i] is XpDrop xpDrop)
                    {
                        xp += xpDrop.Xp;
                        if (xp >= xpToLevelUp)
                        {
                            xp -= xpToLevelUp;
                            level++;
                            xpToLevelUp = (int)(xpToLevelUp * 1.1);
                        }
                    }
                    map.ActualRoom.DropsList = map.ActualRoom.DropsList.Where((drop, index) => index != i).ToArray();
                }
            }


            if (invincibilityTime <= 0)
            {
                foreach (Enemy enemy in map.ActualRoom.Enemies)
                {
                    if (CheckCollision(enemy.Hitbox))
                    {
                        nbHeart--;
                        invincibilityTime = 3000;
                        break;
                    }
                }
            }

            centerPosition = new Vector2(position.X + spriteWidth * scale / 2, position.Y + spriteHeight * scale / 2);
            if (weapon is Bow bow)
            {
                bow.Update(gameTime, map.ActualRoom.Enemies, centerPosition, map.ActualRoom.Tiles);
            }
            else
            {
                weapon.Update(this, direction, map.ActualRoom.Enemies, gameTime);
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new(0, 0, spriteWidth, spriteHeight);
            spriteBatch.Draw(spriteSheetNoMove[currentSpriteSheet], position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0f);

            hitbox = new Rectangle((int)position.X + 5, (int)position.Y + 5, (int)(spriteWidth * scale) - 5, (int)(spriteHeight * scale) - 10);
            DrawRectangle(spriteBatch, hitbox, Color.Red);

            weapon.Draw(spriteBatch);



        }

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)position.X + 5, (int)position.Y + 5, (int)(spriteWidth * scale) - 5, (int)(spriteHeight * scale) - 10);
        }

        private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            spriteBatch.Draw(hitboxTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height), color);
        }
    }
}