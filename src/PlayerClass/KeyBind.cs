using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Dungeon.src.MenuClass.BoutonClass;
using Dungeon.src.TexteClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon.src.PlayerClass
{
    public class KeyBind
    {
        public Dictionary<string, Keys[]> keyBindings = new Dictionary<string, Keys[]>();
        public void ParseData()
        {
            string[] lines = File.ReadAllLines("Content/KeyBind.txt");
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] key = line.Split(' ');
                List<Keys> keysList = new List<Keys>();
                for (int i = 1; i < key.Length; i++)
                {
                    keysList.Add((Keys)Enum.Parse(typeof(Keys), key[i]));
                }
                keyBindings.Add(key[0], keysList.ToArray());
            }
        }

        public void SaveData()
        {
            List<string> lines = [];
            foreach (var binding in keyBindings)
            {
                lines.Add($"{binding.Key} {string.Join(" ", binding.Value)}");
            }
            File.WriteAllLines("Content/keyBind.txt", lines);
        }

        public Keys[] GetKeys(string key)
        {
            return keyBindings[key];
        }

        public void SetKeys(string key, Keys[] keys)
        {
            keyBindings[key] = keys;
        }

        public void ChangeKey(string key, Keys[] newKeys)
        {
            keyBindings[key] = newKeys;
        }
        public void PrintKeyBindings()
        {
            foreach (var binding in keyBindings)
            {
                Console.WriteLine($"{binding.Key}: {string.Join(", ", binding.Value)}");
            }
        }
    }
}