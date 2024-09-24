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

    public class Animation
    {
        private string fileName;
        private AnimationCallback callback;
        private int timeline;
        private int offset;

        private bool paused = false;
        private List<int> framePerTimeLine = new List<int>();
        private List<int> linkToAnotherTimeLine = new List<int>();
        private List<int> timelineFrameTime = new List<int>();
        private Vector2 size;
        private int nbTimeline = 0;
        private int currentFrame = 0;
        private int currentTime;
        private int currentTimeline;
        private int totalAnimationTime = 0;

        private double deltaTime = 0;

        public Texture2D texture;


        public Animation(String fileName, AnimationCallback callback, int timeline = 0, int offset = 0)
        {
            this.fileName = fileName;
            this.callback = callback;
            this.timeline = timeline;
            this.offset = offset;



            if (File.Exists(fileName + ".txt"))
            {
                String[] lines = File.ReadAllLines(fileName + ".txt");
                String[] data = lines[0].Split(' ');

                int width = int.Parse(data[0]);
                int height = int.Parse(data[1]);
                nbTimeline = int.Parse(data[2]);

                size = new Vector2(width, height);

                for (int i = 2; i < lines.Length; i++)
                {
                    data = lines[i].Split(' ');
                    int nbFrame = int.Parse(data[1]);
                    int link = int.Parse(data[2]);
                    int timelineFrame = int.Parse(data[3]);

                    framePerTimeLine.Add(nbFrame);
                    linkToAnotherTimeLine.Add(link);
                    timelineFrameTime.Add(timelineFrame);


                }

                currentFrame = offset / timelineFrameTime[0];

                totalAnimationTime = timelineFrameTime[currentTimeline] * framePerTimeLine[currentTimeline];

                TriggerCallBack();



            }

        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(fileName);
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
            currentTime = (int)(gameTime.ElapsedGameTime.TotalMilliseconds - deltaTime);
            deltaTime = gameTime.ElapsedGameTime.TotalMilliseconds;

            if (linkToAnotherTimeLine[currentTimeline] != -1 && currentTime >= totalAnimationTime)
            {
                offset = currentTime % totalAnimationTime;
                setTimeLine(linkToAnotherTimeLine[currentTimeline], false);
                currentTime = offset;
                currentFrame = -1;
            }
            else
            {
                currentTime %= totalAnimationTime;
            }

            int newFrame = currentTime / timelineFrameTime[currentTimeline];
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