using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon
{
    public class Player
    {
        private const float DefaultScale = 3.0f; // Facteur d'échelle par défaut
        private const float DefaultSpeed = 5.0f; // Vitesse par défaut
        private const int Deadzone = 4096; // Zone morte pour les contrôles de joystick

        public Vector2 Position;
        public float Speed;

        private Texture2D[] spriteSheetNoMove = new Texture2D[3];
        private int currentSpriteSheet = 0;
        private int spriteWidth;
        private int spriteHeight;

        private float scale;

        Rectangle hitbox;

        private Texture2D hitboxTexture;

        public SpriteEffects spriteEffect = SpriteEffects.None;

        public Player(GraphicsDevice graphicsDevice)
        {
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 40;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 40;
            Position = new Vector2(screenWidth / 2 - 65, screenHeight / 2 - 70);
            Speed = DefaultSpeed;
            scale = DefaultScale;

            hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.White });
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

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(spriteWidth * scale), (int)(spriteHeight * scale));
        }

        public bool CheckCollisionWithRoom(Rectangle other)
        {
            Rectangle playerHitbox = GetHitbox();

            // Vérifier la collision avec le bord gauche
            if (playerHitbox.Right > other.Left && playerHitbox.Left < other.Left)
            {
                return true;
            }

            // Vérifier la collision avec le bord droit
            if (playerHitbox.Left < other.Right && playerHitbox.Right > other.Right)
            {
                return true;
            }

            // Vérifier la collision avec le bord supérieur
            if (playerHitbox.Bottom > other.Top && playerHitbox.Top < other.Top)
            {
                return true;
            }

            // Vérifier la collision avec le bord inférieur
            if (playerHitbox.Top < other.Bottom && playerHitbox.Bottom > other.Bottom)
            {
                return true;
            }

            return false;
        }

        public bool CheckCollisionWithEnemy(Rectangle rect)
        {
            Rectangle playerHitbox = GetHitbox();

            if (playerHitbox.Intersects(rect))
            {
                return true;
            }

            return false;
        }

        public bool CheckCollisionWithDoor(Door door, int screenWidth, int screenHeight)
        {

            Rectangle playerHitbox = GetHitbox();

            if (playerHitbox.Intersects(door.hitbox))
            {
                switch (door.direction)
                {
                    case "up":
                        this.Position = new Vector2(Position.X, screenHeight - 175);
                        break;
                    case "down":
                        this.Position = new Vector2(Position.X, 45);
                        break;
                    case "left":
                        this.Position = new Vector2(screenWidth - 150, Position.Y);
                        break;
                    case "right":
                        this.Position = new Vector2(50, Position.Y);
                        break;
                }
                return true;
            }
            return false;
        }


        public void Update(GameTime gameTime, Map map, int screenWidth, int screenHeight)
        {
            var keyboardState = Keyboard.GetState();

            // Définir la hitbox de la salle
            Rectangle roomHitbox = new Rectangle(40, 40, 52 * 35, 30 * 32 + 20);

            // Sauvegarder la position actuelle
            Vector2 previousPosition = Position;

            // Mettre à jour la position en fonction des entrées clavier
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y -= Speed;
                currentSpriteSheet = 2;
                spriteEffect = SpriteEffects.None;
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                Position.Y += Speed;
                currentSpriteSheet = 0;
                spriteEffect = SpriteEffects.None;
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                Position.X -= Speed;
                currentSpriteSheet = 1;
                spriteEffect = SpriteEffects.None;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                Position.X += Speed;
                currentSpriteSheet = 1;
                spriteEffect = SpriteEffects.FlipHorizontally;
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



            if (map.rooms != null && map.currentRoom >= 0 && map.currentRoom < map.rooms.Length)
            {
                Door[] doors = map.rooms[map.currentRoom].doors;
                for (int i = 0; i < doors.Length; i++)
                {
                    if (CheckCollisionWithDoor(doors[i], screenWidth, screenHeight))
                    {
                        map.currentRoom = doors[i].idRoomToGo;


                    }
                }
            }

            if (CheckCollisionWithRoom(roomHitbox))
            {
                Position = previousPosition;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
            spriteBatch.Draw(spriteSheetNoMove[currentSpriteSheet], Position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0f);

            hitbox = new Rectangle((int)Position.X + 5, (int)Position.Y + 5, (int)(spriteWidth * scale) - 10, (int)(spriteHeight * scale) - 10);
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
}