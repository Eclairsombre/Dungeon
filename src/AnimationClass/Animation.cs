using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src
{

    public delegate void AnimationCallback(Vector2 position, Vector2 size);

    public class Animation(string fileName, AnimationCallback callback, int timeline = 0, int offset = 0)
    {
        private string fileName = fileName;
        private AnimationCallback callback = callback;
        private int offset = offset;
        private bool paused = false;
        private List<int> framePerTimeLine = [];
        private List<int> linkToAnotherTimeLine = [];
        private List<int> timelineFrameTime = [];
        private Vector2 size;
        private int nbTimeline = 0;
        private int currentFrame = 0;
        private float currentTime = offset;
        private int currentTimeline = timeline;
        private int totalAnimationTime = 0;

        private double deltaTime = 0;

        public Texture2D texture;

        private float scale = 3.0f;

        public void SetScale(float newScale)
        {
            scale = newScale;
        }

        public float GetScale()
        {
            return scale;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/" + fileName);
        }

        public void ParseData()
        {
            if (File.Exists(@"Content\SpriteData\" + fileName + ".txt"))
            {
                string[] lines = File.ReadAllLines(@"Content\SpriteData\" + fileName + ".txt");
                string[] data = lines[0].Split(' ');

                int width = int.Parse(data[0]);
                int height = int.Parse(data[1]);
                nbTimeline = int.Parse(data[2]);

                size = new Vector2(width, height);

                for (int i = 2; i < lines.Length; i++)
                {
                    data = lines[i].Split(' ');
                    if (data.Length >= 4 && int.TryParse(data[1], out int nbFrame) && int.TryParse(data[2], out int link) && int.TryParse(data[3], out int timelineFrame))
                    {
                        framePerTimeLine.Add(nbFrame);
                        linkToAnotherTimeLine.Add(link);
                        timelineFrameTime.Add(timelineFrame);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid data format in line {i + 1}: {lines[i]}");
                    }




                }

                currentFrame = offset / timelineFrameTime[0];

                totalAnimationTime = timelineFrameTime[currentTimeline] * framePerTimeLine[currentTimeline];

                TriggerCallBack();
            }
        }

        public bool Paused()
        {
            return paused;
        }

        public void SetPaused(bool paused)
        {
            this.paused = paused;
        }

        public int GetTimeline()
        {
            return currentTimeline;
        }

        public int GetTimelineCount()
        {
            return nbTimeline;
        }

        public void ResetTimeLine()
        {
            currentFrame = 0;
            currentTime = 0;
        }

        public void setTimeLine(int newTimeLine, bool triggerCallBack = true)
        {
            if (newTimeLine != currentTimeline && linkToAnotherTimeLine[newTimeLine] != -1)
            {
                ResetTimeLine();
                currentTimeline = newTimeLine;
                totalAnimationTime = timelineFrameTime[currentTimeline] * framePerTimeLine[currentTimeline];
                if (triggerCallBack)
                {
                    TriggerCallBack();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (paused || (framePerTimeLine[currentTimeline] == 1 && linkToAnotherTimeLine[currentTimeline] == -1))
            {
                return;
            }
            currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (linkToAnotherTimeLine[currentTimeline] != -1 && currentTime >= totalAnimationTime)
            {
                offset = (int)(currentTime % totalAnimationTime);
                setTimeLine(linkToAnotherTimeLine[currentTimeline], false);
                currentTime = offset;
                currentFrame = -1;
            }
            else
            {
                currentTime %= totalAnimationTime;
            }

            int newFrame = (int)(currentTime / timelineFrameTime[currentTimeline]);
            if (newFrame != currentFrame)
            {
                currentFrame = newFrame;
                TriggerCallBack();
            }

        }

        public void TriggerCallBack()
        {
            Vector2 position = new Vector2(currentFrame * size.X, currentTimeline * size.Y);
            callback(position, size);
        }

        public Vector2 getSize()
        {
            return size;
        }
    }
}