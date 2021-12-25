/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.Graphics
{
    public static class Sprites
    {

        public static Dictionary<string, Texture2D> LoadedSprites = new Dictionary<string, Texture2D>();
        public static Texture2D MissingTexture;

        #region Load Functions
        // Load Sprite From File
        private static Texture2D LoadTexture2D_FromFile(string FileLocation, GraphicsDevice grafDev)
        {
            FileStream fileStream = new FileStream(FileLocation, FileMode.Open);
            Texture2D ValToReturn = Texture2D.FromStream(grafDev, fileStream);
            fileStream.Dispose();

            return ValToReturn;
        }
        #endregion


        public static void LoadAllSprites(GraphicsDevice grafDev)
        {
            // First, we need to list all files on SPRITES directory
            string[] AllSprites = Directory.GetFiles(Settings.ImagePath, "*.png", SearchOption.AllDirectories);
            Utils.ConsoleWriteWithTitle("SpriteLoader", "Listing all sprites...");

            foreach (var file in AllSprites)
            {
                FileInfo info = new FileInfo(file);
                if (info.Extension != ".png") { continue; }
                string FileFullName = info.FullName;
                string SpriteFiltedName = FileFullName.Replace(Settings.ImagePath, "/").Replace("\\", "/");
    
                if (!LoadedSprites.ContainsKey(SpriteFiltedName))
                {
                    LoadedSprites.Add(SpriteFiltedName, LoadTexture2D_FromFile(FileFullName, grafDev));

                    Utils.ConsoleWriteWithTitle("SpriteLoader", "Found [" + SpriteFiltedName + "].");

                }

            }

            // Set the missing texture
            MissingTexture = GetSprite("/missing_texture.png");

            Utils.ConsoleWriteWithTitle("SpriteLoader", "Operation Completed");
        }


        public static Texture2D GetSprite(string SpriteName)
        {
            string filtredSpriteName = SpriteName;

            if (!SpriteName.EndsWith(".png")) { filtredSpriteName = filtredSpriteName.Insert(filtredSpriteName.Length - 1, ".png"); }
            if (!SpriteName.StartsWith("/")) { filtredSpriteName = filtredSpriteName.Insert(0, "/"); }
            if (!LoadedSprites.ContainsKey(filtredSpriteName)) { return MissingTexture; }
 
            return LoadedSprites[filtredSpriteName];
        }


    }

}