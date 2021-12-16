using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.Graphics
{
    public static class Sprites
    {

        // Sprites Variables
        //public static List<string> AllSpritedLoaded_Names = new List<string>();
        //public static List<Texture2D> AllSpritedLoaded_Content = new List<Texture2D>();
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