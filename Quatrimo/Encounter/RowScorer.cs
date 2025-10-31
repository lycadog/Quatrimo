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
using Quatrimo.Screens;
using Quatrimo.Entities;
using Quatrimo.Entities.block;
using Quatrimo.Encounter;

namespace Quatrimo.Main
{
    public class RowScorer : BlockScorer
    {

        int y;
        List<ScoreIterator> iterators = [];
        List<ScoreIterator> staleIterators = [];
        bool[] processed;
        public bool completed = false;

        public RowScorer(GameScreen screen, int y)
        {
            this.screen = screen;
            this.y = y;
            processed = new bool[screen.boardWidth];
            InitializeIterators();
        }

        public override void Update()
        {
            foreach (var iterator in iterators)
            {
                iterator.Iterate();
            }

            foreach (var stale in staleIterators)
            {
                iterators.Remove(stale);
            }
            staleIterators.Clear();

            if (iterators.Count == 0 && screen.ActiveAnimCount == 0)
            {
                completed = true;
            }
        }

        void InitializeIterators()
        {
            List<Block> immediatelyScoredBlocks = [];

            byte counter = 0; //if 0:
            int outerLeft = screen.boardWidth / 2;
            int outerRight = screen.boardWidth / 2 - 1;
            int innerLeft = 100;
            int innerRight = 100;
            //default values ensure if there is no matching block the animation will start from a set position (the center)

            //if the loop detects a block, then no block, then block again that means there is an empty space
            //which means we need to implement inner iterators. otherwise, if the counter stays at 1, we only need outer iterators
            for (int x = 0; x < screen.boardWidth; x++)
            {
                Block block = screen.blockboard[x, y];

                if (block.justPlaced)
                {
                    ScoreBlock(x);
                }

                switch (counter)
                {

                    case 0:
                        if(block.justPlaced && block.piece == screen.CurrentPiece)
                        {
                            outerLeft = x;
                            outerRight = x;
                            innerLeft = x;
                            counter++;
                        }
                        break;

                    case 1:
                        if(!block.justPlaced || block.piece != screen.CurrentPiece)
                        {
                            innerLeft = x - 1;
                            outerRight = x - 1;
                            counter++;
                        }
                        break;

                    case 2:
                        if(block.justPlaced && block.piece == screen.CurrentPiece)
                        {
                            outerRight = x;
                            innerRight = x;
                            counter++;
                        }
                        break;

                    case 3:
                        if (block.justPlaced && block.piece == screen.CurrentPiece)
                        {
                            outerRight = x;
                        }
                        break;
                }
            }

            //create outer iterators
            iterators.Add(new ScoreIterator(this, outerLeft, -1));
            iterators.Add(new ScoreIterator(this, outerRight, 1));

            if (counter < 3) //if there are no empty spaces in the current piece we do not need inner iterators, so return
            {
                return;
            }
            iterators.Add(new ScoreIterator(this, innerLeft, 1));
            iterators.Add(new ScoreIterator(this, innerRight, -1));
        }

        void QueueIteratorRemoval(ScoreIterator iterator)
        {
            staleIterators.Add(iterator);
        }

        void ScoreBlock(int x, int index = 0)
        {
            processed[x] = true;
            screen.ScoreBlock(screen.blockboard[x, y], index);
        }

        class ScoreIterator
        {
            RowScorer scorer;
            int x, direction;
            float iterationCooldown = 0;

            public ScoreIterator(RowScorer scorer, int x, int direction)
            {
                this.scorer = scorer;
                this.x = x;
                this.direction = direction;
            }

            public void Iterate()
            {
                if (iterationCooldown < .025)
                {
                    iterationCooldown += TimeManager.SecondDifference;
                    return;
                }
                iterationCooldown = 0;
                x += direction;

                if (x >= scorer.screen.boardWidth || x < 0)
                {
                    Terminate();
                    return;
                }

                if (scorer.processed[x]) //if this spot has already been processed, don't process it again
                {
                    return;
                }

                //if iterator has reached either edge of the board, stop iterating
                
                scorer.ScoreBlock(x);
            }

            void Terminate()
            {
                scorer.QueueIteratorRemoval(this);
            }
        }
    }
}
