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
    public partial class RollingNumberBox
    {
        int finalValue;
        int currentValue;
        int displayedNumber;
        int staleNumber;

        float baseSpeed = 1;

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
            //roll towards finalValue, changing currentValue when number updates, rolling up or down,
            //as well as firing cycle events. these need specification to be up or down as well

            //run the below code ONLY if the animation is active!!!

            baseSpeed *= Math.Sign(finalValue - currentValue); //set speed to positive, negative, or neutral depending on rolls left

            if (BoxSprite.JustChangedFrame)//check current animation frame and get number.
            {
                displayedNumber = BoxSprite.CurrentFrameIndex / 11;

                

                if (displayedNumber != staleNumber) //if the number has changed outside of cycling
                {
                    currentValue += Math.Sign(BoxSprite.AnimationSpeed);

                    //if the animation has looped,
                    if (BoxSprite.JustCycled)
                    {
                        //fire rollover event
                    }
                }

                if (currentValue == finalValue) { SetToNumberAndPause(Math.Abs(finalValue % 10)); }
            }
        }

        public void SetToNumberAndPause(int number)
        {
            BoxSprite.CurrentFrameIndex = number * 11;
            BoxSprite.Animate = false;
            //add code to change state to stationary
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
