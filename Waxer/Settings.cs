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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waxer
{
    public static class Settings
    {
        // Information
        public static string DevelopmentBranch = "pre-alpha";
        public static string Version = "0.1";
        public static string VersionString = $"{Version}_{DevelopmentBranch}";
 
        public static float AnimationSpeed = 25f;
        public static bool DebugMode = true;

        public static string ContentFolder = Path.Combine(Environment.CurrentDirectory, "Waxer_data");
        public static string FontsPath = Path.Combine(ContentFolder, "fonts");
        public static string ImagePath = Path.Combine(ContentFolder, "img");
        public static string SoundPath = Path.Combine(ContentFolder, "sound");
        public static string DataPath = Path.Combine(ContentFolder, "data");
         
        // Debug options 
        public static bool Debug_RenderColidersTiles = true;
        public static bool Debug_RenderPlayerPositionPoints = true;
        public static bool Debug_RenderItemsRangeBox = true;
        public static bool Debug_DrawChunkBorders = true;
        
        //Physics
        public static float StableDelta = 0.016f;
        public static float MaximumMoveSpeed = 1024;

    }
}
