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

namespace Dungeon.src.PlayerClass
{
    public class Player
    {
        private const float DefaultScale = 3.0f;
        private const float DefaultSpeed = 10.0f;
        private const int Deadzone = 4096;
        private Vector2 position, centerPosition, startPosition, direction, directionDeplacement;
        private readonly float speed;
        private float scale;
        private readonly Texture2D[] spriteSheetNoMove = new Texture2D[3];
        private readonly Texture2D hitboxTexture;
        private int currentSpriteSheet;
        private SpriteEffects spriteEffect = SpriteEffects.None;
        private int spriteWidth, spriteHeight;
        private Rectangle hitbox, rangeInFrontPlayer;
        private Weapon weapon;
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
        public Vector2 CenterPosition { get { return centerPosition; } set { centerPosition = value; } }
        public Vector2 Direction { get { return direction; } set { direction = value; } }

        private readonly KeyBind keyBind;
        public Player(GraphicsDevice graphicsDevice, KeyBind keyBind)
        {
            this.keyBind = keyBind;
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 40;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 40;
            position = new Vector2(screenWidth / 2 - 65, screenHeight - 200);
            startPosition = new Vector2(screenWidth / 2 - 65, screenHeight - 200);
            speed = DefaultSpeed;
            scale = DefaultScale;
            centerPosition = new Vector2((position.X + spriteWidth) * scale / 2, (position.Y + spriteHeight) * scale / 2);
            hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });
            direction = new Vector2(0, -1);
            directionDeplacement = new Vector2(0, 0);
            weapon = new Bow(centerPosition);
            hitbox = new Rectangle((int)position.X + 5, (int)position.Y + 5, (int)(spriteWidth * scale) - 5, (int)(spriteHeight * scale) - 10);
            rangeInFrontPlayer = Rectangle.Empty;
            spriteSheetNoMove = new Texture2D[3];

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
        }

        public void Equip(Weapon weapon)
        {
            this.weapon = weapon;
        }

        public void PlayerKeyboardAndMouseInput(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(keyBind.keyBindings["Use"][0]) || (keyBind.keyBindings["Use"].Length > 1 && keyboardState.IsKeyDown(keyBind.keyBindings["Use"][1])))
            {
                if (useCooldownTimer >= useCooldown)
                {
                    useCooldownTimer = 0;
                    rangeInFrontPlayer = direction switch
                    {
                        Vector2 v when v == new Vector2(0, -1) => new Rectangle((int)position.X, (int)position.Y - spriteHeight, (int)(SpriteWidth * scale), spriteHeight), // up
                        Vector2 v when v == new Vector2(0, 1) => new Rectangle((int)position.X, (int)position.Y + (int)(spriteHeight * scale), (int)(SpriteWidth * scale), spriteHeight), // down
                        Vector2 v when v == new Vector2(1, 0) => new Rectangle((int)position.X + (int)(spriteWidth * scale), (int)position.Y, spriteWidth, (int)(SpriteHeight * scale)), // right
                        Vector2 v when v == new Vector2(-1, 0) => new Rectangle((int)position.X - spriteWidth, (int)position.Y, spriteWidth, (int)(SpriteHeight * scale)), // left
                        _ => new Rectangle((int)position.X, (int)position.Y, 100, 100),
                    };
                }
            }
            else if (keyboardState.IsKeyUp(Keys.E) && useCooldownTimer < useCooldown)
            {
                useCooldownTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                rangeInFrontPlayer = Rectangle.Empty;
            }

            UpdateMovement(keyboardState, keyBind.keyBindings["Up"][0], keyBind.keyBindings["Up"].Length > 1 ? keyBind.keyBindings["Up"][1] : Keys.None, new Vector2(0, -1), 2, SpriteEffects.None);
            UpdateMovement(keyboardState, keyBind.keyBindings["Down"][0], keyBind.keyBindings["Down"].Length > 1 ? keyBind.keyBindings["Down"][1] : Keys.None, new Vector2(0, 1), 0, SpriteEffects.None);
            UpdateMovement(keyboardState, keyBind.keyBindings["Left"][0], keyBind.keyBindings["Left"].Length > 1 ? keyBind.keyBindings["Left"][1] : Keys.None, new Vector2(-1, 0), 1, SpriteEffects.None);
            UpdateMovement(keyboardState, keyBind.keyBindings["Right"][0], keyBind.keyBindings["Right"].Length > 1 ? keyBind.keyBindings["Right"][1] : Keys.None, new Vector2(1, 0), 1, SpriteEffects.FlipHorizontally);

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

        private void UpdateMovement(KeyboardState keyboardState, Keys primaryKey, Keys secondaryKey, Vector2 directionVector, int spriteIndex, SpriteEffects effect)
        {
            if (keyboardState.IsKeyDown(primaryKey) || keyboardState.IsKeyDown(secondaryKey))
            {
                if (directionVector.X != 0) isMovingHorizontally = true;
                if (directionVector.Y != 0) isMovingVertically = true;
                currentSpriteSheet = spriteIndex;
                spriteEffect = effect;
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
            if (invincibilityTime <= 0)
            {
                foreach (Enemy enemy in map.ActualRoom.Enemies)
                {
                    if (Collision.CheckCollisionTwoRect(hitbox, enemy.Hitbox))
                    {
                        stats.Health--;
                        invincibilityTime = 3000;
                        break;
                    }
                }
            }
            (bool isDoor, Door door) = Collision.CheckCollisionWithDoor(newHitbox, map.ActualRoom);
            if (isDoor)
            {
                map.ActualRoom = new Room(door.RewardType);
                Random random = new();
                int roomNumber = random.Next(2, 6);
                map.ActualRoom.LoadContent(content, roomNumber);
                map.ActualRoom.Generate(content);
                position = startPosition;
            }
            else

            {
                if (!Collision.CheckCollisionWithRoom(newHitbox, map.ActualRoom))
                {
                    position = futurePosition;
                    hitbox = newHitbox;
                }

                for (int i = 0; i < map.ActualRoom.DropsList.Length; i++)
                {
                    if (Collision.CheckCollisionTwoRect(map.ActualRoom.DropsList[i].Hitbox, hitbox))
                    {
                        if (map.ActualRoom.DropsList[i] is XpDrop xpDrop)
                        {
                            stats.Xp += xpDrop.Xp;
                            stats.Update(gameTime, ref gameState);
                            map.ActualRoom.DropsList = map.ActualRoom.DropsList.Where((drop, index) => index != i).ToArray();

                        }
                        else if (map.ActualRoom.DropsList[i] is HeartDrop heartDrop)
                        {
                            if (stats.Health < stats.MaxHealth)
                            {
                                stats.Health++;
                                map.ActualRoom.DropsList = map.ActualRoom.DropsList.Where((drop, index) => index != i).ToArray();

                            }
                        }
                    }
                }
                for (int i = 0; i < map.ActualRoom.Tiles.GetLength(0); i++)
                {
                    for (int y = 0; y < map.ActualRoom.Tiles.GetLength(1); y++)
                    {
                        if (map.ActualRoom.Tiles[i, y].Id.Item1 == 4 && map.ActualRoom.Finished)
                        {
                            if (rangeInFrontPlayer.Intersects(map.ActualRoom.Tiles[i, y].Holder.Hitbox))
                            {
                                if (map.ActualRoom.Tiles[i, y].Holder is WeaponHolder weaponHolder)
                                {
                                    Weapon weapon = weaponHolder.SwitchWeapon(this.weapon);
                                    Equip(weapon);
                                }
                                else if (map.ActualRoom.Tiles[i, y].Holder is DropHolder dropHolder)
                                {
                                    if (dropHolder.DropHold is XpDrop xpDrop)
                                    {
                                        stats.Xp += xpDrop.Xp;
                                        stats.Update(gameTime, ref gameState);
                                        map.ActualRoom.Tiles[i, y].Holder.DropHold = null;

                                    }
                                    else if (dropHolder.DropHold is HeartDrop heartDrop)
                                    {
                                        if (stats.Health < stats.MaxHealth)
                                        {
                                            stats.Health++;
                                            map.ActualRoom.Tiles[i, y].Holder.DropHold = null;

                                        }
                                    }
                                    else if (dropHolder.DropHold is GoldDrop goldDrop)
                                    {
                                        stats.Gold += goldDrop.Amount;
                                        map.ActualRoom.Tiles[i, y].Holder.DropHold = null;

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void Update(GameTime gameTime, Map map, ContentManager content, ref GameState gameState)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invincibilityTime > 0)
            {
                invincibilityTime -= (int)(deltaTime * 1000);
            }

            Vector2 futurePosition = position;
            directionDeplacement = new Vector2(0, 0);
            PlayerKeyboardAndMouseInput(gameTime);
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

            centerPosition = new Vector2(position.X + spriteWidth * scale / 2, position.Y + spriteHeight * scale / 2);
            weapon.Update(this, gameTime, map, content);
            Deplacement(ref futurePosition);
            Rectangle newHitbox = new((int)futurePosition.X + 10, (int)futurePosition.Y + 10, (int)(spriteWidth * scale) - 10, (int)(spriteHeight * scale) - 15);
            PlayerCollision(map, content, newHitbox, futurePosition, gameTime, ref gameState);

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.U))
            {
                for (int i = 0; i < map.ActualRoom.Enemies.Count; i++)
                {
                    map.ActualRoom.Enemies[i].Hp = 0;
                }
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
            Rectangle sourceRectangle = new(0, 0, spriteWidth, spriteHeight);
            spriteBatch.Draw(spriteSheetNoMove[currentSpriteSheet], position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0f);

            DrawRectangle(spriteBatch, hitbox, Color.Red);
            weapon.Draw(spriteBatch);

            if (rangeInFrontPlayer != Rectangle.Empty)
            {
                DrawRectangle(spriteBatch, rangeInFrontPlayer, Color.Blue);
            }
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