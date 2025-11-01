using Microsoft.Xna.Framework;
using Quatrimo.Entities.block;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Encounter
{
    public class BlockScorer : Scorer
    {
        Block[] blocks;

        public BlockScorer(GameScreen screen, Block[] blocks)
        {
            this.screen = screen;
            this.blocks = blocks;
            Start();
        }

        public BlockScorer(GameScreen screen, Point[] positions)
        {
            this.screen = screen;
            blocks = new Block[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                blocks[i] = screen.blockboard[positions[i].X, positions[i].Y];
            }
            Start();
        }

        public override void Update()
        {
            
        }

        protected override void Start()
        {
            foreach(var block in blocks)
            {
                if (block.scored) { continue; }
                screen.ScoreBlock(block);
            }
            completed = true;
        }
    }
}
