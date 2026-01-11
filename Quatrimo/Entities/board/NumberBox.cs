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
    public partial class NumberBox
    {
        int totalRolls;
        int currentRolls;
        int displayedNumber;
        int staleNumber;
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
            

            if (BoxSprite.JustChangedFrame)//check current animation frame and get number.
            {
                displayedNumber = BoxSprite.CurrentFrameIndex / 11;

                //we need a way to update current rolls! if the number displayed changes we need to figure that out and update current rolls!
                //check if the animation should be paused on the current number or should keep going


                //if the animation has looped,
                if (BoxSprite.JustCycled)
                {
                    //fire rollover event
                }
            }
        }

        /// <summary>
        /// Resets state to default
        /// </summary>
        public void Reset(bool markInactive = false)
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
