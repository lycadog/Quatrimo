using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using Microsoft.Xna.Framework;
using Quatrimo.Encounter;

namespace Quatrimo.Entities.block
{
    public partial class BombBlock
    {
        public int timer = 4;

        public override void Score(bool forcedRemoval = false)
        {
            base.Score(forcedRemoval);
            explode();
        }

        protected override void CustomTick()
        {
            timer -= 1;
            if (timer == 0)
            {
                explode();
            }
        }

        void explode()
        {

            Point[] positions = [
                new(boardX - 1, boardY - 1), new(boardX - 1, boardY), new(boardX - 1, boardY + 1),
                new(boardX,boardY), new(boardX, boardY-1), new(boardX, boardY + 1),
                new(boardX+1, boardY-1), new(boardX+1, boardY), new(boardX + 1, boardY+1)];

            screen.activeScorers.Add(
                new BlockScorer(screen, positions)
                );
        }

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
    }
}
