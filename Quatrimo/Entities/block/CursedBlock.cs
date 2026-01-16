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

namespace Quatrimo.Entities.block
{
    public partial class CursedBlock
    {
        int openDuration = 0;
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
            
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

        public override void Score(bool forcedRemoval = false)
        {
            if (forcedRemoval)
            {
                base.Score(forcedRemoval);
            }
            scored = true;
            Layer1LeftTexture = 150;
            Layer2LeftTexture = 150;
            openDuration = 3;
            score += 2;
        }

        public override void RemovePlaced(bool forced = false)
        {
            if (forced)
            {
                base.RemovePlaced(forced);
            }
        }

        public override void RemoveAndLower(bool forced = false)
        {
            if (forced)
            {
                base.RemoveAndLower(forced);
            }
            else { scored = false; }
        }

        protected override void CustomTick()
        {
            openDuration -= 1;
            if(openDuration == 0)
            {
                Layer1LeftTexture = 140;
                Layer2LeftTexture = 140;
                score -= 2;
            }
        }
    }
}
