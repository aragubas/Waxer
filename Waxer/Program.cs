/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using Microsoft.Xna.Framework;
using System;
using MonoGame.Framework;

namespace Waxer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool Summary = true;

            // Listen for switches
            foreach(string _switch in args)
            {
                if (_switch == "no-summary" || _switch == "--no-summary") { Summary = false; }
            }

            if (Summary)
            {
                WriteSummary();
            }
 
            // Creates a instance of game
            Game game = new GameMain();
            game.Run();
        }

        static void WriteSummary()
        {
            Console.WriteLine($"Waxer {Settings.VersionString} (Aragubas 2019-2022)");
            Console.WriteLine("-:Flags:");

            // Write Debug Mode Flags
            if (Settings.DebugMode) 
            { 
                Console.WriteLine("--DebugMode"); 
                if (Settings.Debug_RenderColidersTiles) { Console.WriteLine("---RenderColiderTiles"); }
                if (Settings.Debug_RenderItemsRangeBox) { Console.WriteLine("---RenderItemsRangeBox"); }
                if (Settings.Debug_RenderPlayerPositionPoints) { Console.WriteLine("---RenderPlayerPositionPoints"); }
                if (Settings.Debug_RenderPlayerPositionPoints) { Console.WriteLine("---RenderChunkBorders"); }
            }
            
            Console.WriteLine("THIS RELEASE IS NOT ALLOWED FOR DISTRIBUTION NOR REVERSE ENGINNERING.");
        }

    }
}

