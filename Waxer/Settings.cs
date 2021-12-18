using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waxer
{
    public static class Settings
    {
        public static float AnimationSpeed = 25f;
        public static bool DebugMode = true;

        public static string ContentFolder = $"{Environment.CurrentDirectory}\\Waxer_data\\";
        public static string FontsPath = ContentFolder + "fonts\\";
        public static string ImagePath = ContentFolder + "img\\";
        public static string SoundPath = ContentFolder + "sound\\";
        public static string ConfigPath = ContentFolder + "config\\";
        
        // Debug options
        public static bool Debug_RenderColidersTiles = false;
        public static bool Debug_RenderPlayerPositionPoints = false;
        public static bool Debug_RenderItemsRangeBox = true;
        

    }
}
