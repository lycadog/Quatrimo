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
                sprite.RelativeX = -(i * 10);
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
            //TODO: check if number has too many digits before iterating. if so change functionality

            int quotient = number, remainder;

            for(int i = 0; i < Digits; i++)
            {
                quotient = Math.DivRem(quotient, 10, out remainder); //divide by 10, use the remainder for the box. then remove
                SetBox(i, remainder);
                if(quotient == 0) { return; }
            }
        }

        public void ClearBox()
        {
            foreach(Sprite sprite in sprites)
            {
                sprite.LeftTexturePixel = 100;
                sprite.RightTexturePixel = 111;
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
            foreach(var sprite in sprites)
            {
                SpriteManager.RemoveSprite(sprite);
            }
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

    }
}
