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
using Quatrimo.Data;

namespace Quatrimo.Entities
{
    public partial class ScoreAnimation
    {

        public void StartScoreAnimation(int x, int y, int index = 0)
        {
            SpriteInstance.CurrentChain = SpriteInstance.AnimationChains[index];
            RelativeX = x * 10 + 10; RelativeY = y * 10 + 10;
            RelativeZ = 4;

            SpriteInstance.Animate = true;
        }

        public void SetColor(HsvColor color)
        {
            SpriteInstance.Color = color.color;
        }
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            
        }

        private void CustomActivity()
        {
            if (SpriteInstance.JustCycled)
            {
                SpriteInstance.Visible = false;
                Destroy();
            }
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
