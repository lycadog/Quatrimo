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

namespace Quatrimo.Entities.board
{
    public partial class RowScorer
    {
        GameScreen screen;

        int y;
        List<ScoreIterator> iterators = [];
        List<ScoreIterator> staleIterators = [];
        List<int> processedXValues = [];
        public bool completed = false;

        public RowScorer(GameScreen screen, int y)
        {
            this.screen = screen;
            this.y = y;
            InitializeIterators();

        }

        void InitializeIterators()
        {



            byte counter = 0; //if 0:
            int outerLeft = screen.boardWidth / 2;
            int outerRight = screen.boardWidth / 2;
            int innerLeft = 100;
            int innerRight = 100;
            //default values ensure if there is no matching block the animation will start from a set position (the center)

            //if the loop detects a block, then no block, then block again that means there is an empty space
            //which means we need to implement inner iterators. otherwise, if the counter stays at 1, we only need outer iterators
            for (int x = 0; x < screen.boardWidth; x++)
            {

                if (screen.blockboard[x, y].justPlaced)
                {

                }

                switch (counter)
                {
                    case 0:
                        if (screen.blockboard[x, y].justPlaced)
                        {
                            counter++;
                            outerLeft = x;
                        }break;

                    case 1:
                        outerRight = x;
                        if (!screen.blockboard[x,y].justPlaced)
                        {
                            innerLeft = x - 1;
                            outerRight = x - 1;
                            counter++;
                        }break;

                    case 2:
                        if (screen.blockboard[x,y].justPlaced)
                        {
                            innerRight = x;
                            outerRight = x;
                            counter++;
                        }
                        break;

                    case 3:
                        if (screen.blockboard[x, y].justPlaced)
                        {
                            outerRight = x;                      
                        }
                        break;
                }
            }

            //create outer iterators
            iterators.Add(new ScoreIterator(this, outerLeft, y, -1));
            iterators.Add(new ScoreIterator(this, outerRight, y, 1));

            if (counter < 3) //if there are no empty spaces in the current piece we do not need inner iterators, so return
            {
                return;
            }
            iterators.Add(new ScoreIterator(this, innerLeft, y, 1));
            iterators.Add(new ScoreIterator(this, innerRight, y, -1));
        }

        private void CustomInitialize()
        {
            
        }

        private void CustomActivity()
        {
            foreach(var iterator in iterators)
            {
                iterator.Iterate();
            }

            foreach(var stale in staleIterators)
            {
                iterators.Remove(stale);
            }
            staleIterators.Clear();

            if (ActiveAnims.Count == 0)
            {
                completed = true;
            }

        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

        void QueueIteratorRemoval(ScoreIterator iterator)
        {
            staleIterators.Add(iterator);
        }

        void ScoreBlock(int x, int y, int index = 0)
        {
            
            ScoreAnimation anim = Factories.ScoreAnimationFactory.CreateNew();
            anim.StartScoreAnimation(x, y, index);
        }

        class ScoreIterator(RowScorer Scorer, int X, int Y, int Direction)
        {
            RowScorer scorer => Scorer;
            int x => X;
            int y => Y;
            int direction => Direction;
            float iterationCooldown = 0;

            public void Iterate()
            {
                if (iterationCooldown < .25)
                {
                    iterationCooldown += TimeManager.SecondDifference;
                    return;
                }
                iterationCooldown = 0;

                //iterate !!!
                x += direction;
                if (scorer.processedXValues.Contains(x)) //if this spot has already been processed, don't process it again
                {
                    return;
                }

                //iterate here now

            }

            void Terminate()
            {
                scorer.QueueIteratorRemoval(this);
            }
        }
    }
}
