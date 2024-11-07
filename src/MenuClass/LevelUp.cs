using System;
using Dungeon.src.AnimationClass;
using Dungeon.src.MenuClass.BoutonClass;
using Dungeon.src.PlayerClass.StatsClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.MenuClass
{


    public class LevelUp
    {
        private readonly Animation animation;
        private readonly CallBack callBack;
        private Rectangle hitbox;
        private readonly LevelUpBouton choice1, choice2, choice3;
        public LevelUp(GraphicsDevice graphicsDevice, ContentManager content)
        {
            callBack = new CallBack();
            animation = new Animation("LevelUpBackground-Sheet", callBack.StaticMyCallback, 1, 0);
            animation.ParseData();

            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            int hitboxWidth = 800;
            int hitboxHeight = 800;

            hitbox = new Rectangle(
                (screenWidth - hitboxWidth) / 2,
                (screenHeight - hitboxHeight) / 2,
                hitboxWidth,
                hitboxHeight
            );

            int hitboxChoiceWidth = 400;
            int hitboxChoiceHeight = 150;

            Random random = new();
            string[] choices = ["LevelUpHealBouton-Sheet", "LevelUpAttackBouton-Sheet", "LevelUpSpeedBouton-Sheet"];


            double[] chancePourcentages = [0.6, 0.2, 0.1, 0.05, 0.01];
            double[] healthBoosts = [0.5, 1, 1.5, 2, 3];
            double[] attackBoosts = [1.1, 1.2, 1.3, 1.4, 1.5];
            double[] speedBoosts = [1.1, 1.2, 1.3, 1.4, 1.5];


            int choice1FileIndex = random.Next(choices.Length);
            int choice2FileIndex = random.Next(choices.Length);
            int choice3FileIndex = random.Next(choices.Length);

            string choice1File = choices[choice1FileIndex];
            string choice2File = choices[choice2FileIndex];
            string choice3File = choices[choice3FileIndex];

            double choice1Boost = 0.0;
            double choice2Boost = 0.0;
            double choice3Boost = 0.0;

            choice1Boost = choice1FileIndex switch
            {
                0 => GetBoost(random, chancePourcentages, healthBoosts),
                1 => GetBoost(random, chancePourcentages, attackBoosts),
                2 => GetBoost(random, chancePourcentages, speedBoosts),
                _ => 0.0
            };

            choice2Boost = choice2FileIndex switch
            {
                0 => GetBoost(random, chancePourcentages, healthBoosts),
                1 => GetBoost(random, chancePourcentages, attackBoosts),
                2 => GetBoost(random, chancePourcentages, speedBoosts),
                _ => 0.0
            };

            choice3Boost = choice3FileIndex switch
            {
                0 => GetBoost(random, chancePourcentages, healthBoosts),
                1 => GetBoost(random, chancePourcentages, attackBoosts),
                2 => GetBoost(random, chancePourcentages, speedBoosts),
                _ => 0.0
            };


            choice1 = new LevelUpBouton((screenWidth - hitboxChoiceWidth) / 2, hitbox.Y + 200, hitboxChoiceWidth, hitboxChoiceHeight, GameState.Playing, choice1File, (LevelUpChoice)choice1FileIndex, choice1Boost, content);
            choice2 = new LevelUpBouton((screenWidth - hitboxChoiceWidth) / 2, hitbox.Y + 350, hitboxChoiceWidth, hitboxChoiceHeight, GameState.Playing, choice2File, (LevelUpChoice)choice2FileIndex, choice2Boost, content);
            choice3 = new LevelUpBouton((screenWidth - hitboxChoiceWidth) / 2, hitbox.Y + 500, hitboxChoiceWidth, hitboxChoiceHeight, GameState.Playing, choice3File, (LevelUpChoice)choice3FileIndex, choice3Boost, content);
        }

        public void LoadContent(ContentManager content)
        {
            animation.LoadContent(content);

            choice1.LoadContent(content);
            choice2.LoadContent(content);
            choice3.LoadContent(content);
        }

        private double GetBoost(Random random, double[] chancePourcentages, double[] boosts)
        {
            double randomValue = random.NextDouble();
            double cumulative = 0.0;

            for (int i = 0; i < chancePourcentages.Length; i++)
            {
                cumulative += chancePourcentages[i];
                if (randomValue < cumulative)
                {
                    return boosts[i];
                }
            }

            return 0.0;
        }

        public void Update(GameTime gameTime, ref GameState gameState, ref GameState previousGameState, ref Stats stats)
        {
            animation.Update(gameTime);
            choice1.Update(gameTime);
            choice2.Update(gameTime);
            choice3.Update(gameTime);

            choice1.OnClick(ref gameState, ref previousGameState, ref stats);
            choice2.OnClick(ref gameState, ref previousGameState, ref stats);
            choice3.OnClick(ref gameState, ref previousGameState, ref stats);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.texture, new Vector2(hitbox.X, hitbox.Y), callBack.SourceRectangle, Color.White);
            choice1.Draw(spriteBatch);
            choice2.Draw(spriteBatch);
            choice3.Draw(spriteBatch);
        }
    }
}