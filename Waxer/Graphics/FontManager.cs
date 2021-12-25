/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Waxer.Graphics
{
    public class FontDescriptor
    {
        public string Name;
        public float Size;
        public FontDescriptor(string FontName, float FontSize) { Name = FontName; Size = FontSize; }
    }

    public static class Fonts
    {

        // Sprites Variables
        public static Dictionary<FontDescriptor, SpriteFont> LoadedFonts = new Dictionary<FontDescriptor, SpriteFont>();
        public static float SmallFontSize = 8;
        public static float TyneFontSize = 7.8f;
        public static float DefaultFontSize = 10;
        public static SpriteFont DefaultFont;

        public static SpriteFont BakeFont(string FontName, float FontSize, GraphicsDevice graphDev)
        {
            Utils.ConsoleWriteWithTitle("Fonts.BakeFont", $"Baker Font '{FontName}' with Size '{FontSize.ToString()}'", true);
            TtfFontBakerResult fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes($"{Settings.FontsPath}{FontName}.ttf"),
                FontSize,
                1024,
                1024,
                new[]
                {
                    CharacterRange.BasicLatin,
                    CharacterRange.Latin1Supplement,
                    CharacterRange.LatinExtendedA,
                    CharacterRange.Cyrillic
                }
            );
            Utils.ConsoleWriteWithTitle("Fonts.BakeFont", "Complete.", true);

            return fontBakeResult.CreateSpriteFont(graphDev);
        }

        public static void PrecacheFont(string FontName, float FontSize, GraphicsDevice grafDev)
        {
            foreach (var Ceira in LoadedFonts.Keys)
            {
                if (Ceira.Name == FontName && Ceira.Size == FontSize) { return; }
            }

            FontDescriptor ceira = new FontDescriptor(FontName, FontSize);
            LoadedFonts.Add(ceira, BakeFont(FontName, FontSize, grafDev));
        }

        public static FontDescriptor GetFontDescriptor(string FontName, float FontSize, GraphicsDevice grafDev)
        {
            foreach (var Ceira in LoadedFonts.Keys)
            {
                if (Ceira.Name == FontName && Ceira.Size == FontSize) { return Ceira; }
            }

            FontDescriptor ceira = new FontDescriptor(FontName, FontSize);
            LoadedFonts.Add(ceira, BakeFont(FontName, FontSize, grafDev));
            return ceira;
        }

        public static void LoadDefaultFont()
        {
            
        }

        public static SpriteFont GetSpriteFont(FontDescriptor descriptor)
        {
            if (LoadedFonts.ContainsKey(descriptor)) { return LoadedFonts[descriptor]; }
            throw new FileNotFoundException($"Font descriptor was not loaded before\nDescriptorInfo:\nFontName: {descriptor.Name},FontSize:{descriptor.Size}");
        }


    }

}