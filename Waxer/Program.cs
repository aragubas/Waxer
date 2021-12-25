/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using Microsoft.Xna.Framework;
using MonoGame.Framework;

namespace Waxer
{
    public class Program
    {
        public static void Main(string[] Args)
        {
            // Creates a instance of game
            Game game = new GameMain();
            game.Run();
        }
    }
}

