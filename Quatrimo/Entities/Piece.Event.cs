using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Screens;
using Quatrimo.Entities.block;
using Quatrimo.Entities.board;
using Quatrimo.Screens;
namespace Quatrimo.Entities
{
    public partial class Piece
    {
        void OnAfterboardXSet (object sender, EventArgs e) 
        {
            RelativeX = boardX * 10 + 10; 
        }
        void OnAfterboardYSet(object sender, EventArgs e)
        {
            RelativeY = boardY * 10 + 10;
        }

    }
}
