using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;

namespace Quatrimo.Entities.board
{
    public partial class NumberBar
    {
        Sprite[] sprites;
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            sprites = new Sprite[Digits];
            for(int i = 0; i < Digits; i++)
            {
                Sprite sprite = SpriteManager.AddSprite(numberBox, LayerProvidedByContainer);
                sprite.AttachTo(this);
                sprite.RelativeX = i * 10;
                sprites[i] = sprite;

                sprite.LeftTexturePixel = 100;
                sprite.RightTexturePixel = 111;
                sprite.BottomTexturePixel = 14;
                sprite.Width = 11;
                sprite.Height = 14;
            }

        }

        public void UpdateBoxes(int number)
        {
            for(int i = 0; i < Digits; i++)
            {

            }
        }

        void SetBox(int index, int number)
        {
            sprites[index].LeftTexturePixel = 10 * number;
            sprites[index].RightTexturePixel = sprites[index].LeftTexturePixel + 11;
        }

        private void CustomActivity()
        {
            
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
