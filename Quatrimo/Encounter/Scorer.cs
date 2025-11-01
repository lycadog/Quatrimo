using FlatRedBall.Screens;
using Quatrimo.Entities.block;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Encounter
{
    public abstract class Scorer
    {
        protected GameScreen screen;
        public bool completed = false;

        public abstract void Update();

        protected abstract void Start();

        protected virtual void ProcessBlock(Block block)
        {
            if (block.scored) { return; }
            screen.ScoreBlock(block);
        }
    }
}
