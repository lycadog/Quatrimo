using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Screens;
using Quatrimo.Entities;
using Quatrimo.Screens;
namespace Quatrimo.Entities.block
{
    public partial class Block
    {
        void OnAfterLayer1LeftTextureSet (object sender, EventArgs e) 
        {
            SpriteLayer1.RightTexturePixel = Layer1LeftTexture + 10;
        }
        void OnAfterLayer1TopTextureSet (object sender, EventArgs e) 
        {
            SpriteLayer1.BottomTexturePixel = Layer1TopTexture + 10;
        }
        void OnAfterLayer2LeftTextureSet (object sender, EventArgs e) 
        {
            SpriteLayer2.RightTexturePixel = Layer2LeftTexture + 10;
        }
        void OnAfterLayer2TopTextureSet (object sender, EventArgs e) 
        {
            SpriteLayer2.BottomTexturePixel = Layer2TopTexture + 10;
        }
        void OnAfterLayer3LeftTextureSet (object sender, EventArgs e) 
        {
            SpriteLayer3.RightTexturePixel = Layer3LeftTexture + 10;
        }
        void OnAfterLayer3TopTextureSet(object sender, EventArgs e)
        {
            SpriteLayer3.BottomTexturePixel = Layer3TopTexture + 10;
        }
 
    }
}
