using FlatRedBall;
using Quatrimo.Data;
using Quatrimo.Entities.block;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class BlockPlacementAttack : EnemyAttack
    {
        public int[] blockTypes;
        public int[] placementPositions;

        // Range of blocks dropped
        public int minBlocksDropped = 3;
        public int maxBlocksDropped = 10;
        public int repeats = 0; //number of times to repeat the attack

        public bool depthRestricted = true; // Whether or not the depth values are used to restrict block depth
        public bool depthEnforced; // Whether or not all blocks dropped match the depth perfectly

        //Range of allowed depth
        public int depthMin;
        public int depthMax;

        //Misc toggles
        public bool clustered; //Places blocks near dense clusters
        public bool symmetrical; //Places blocks symmetrically, enforces even number of blocks
        public bool randomizePositions = true;


        public int textureX;
        public int textureY;
        public HsvColor color;
        public bool useRandomColor = true;

        //add visual configurations!
        
        public enum PlacementType
        {
            DroppedFromAbove,
            RaisedFromBelow,
        }

        public override void ExecuteAttack(GameScreen screen, Enemy enemy)
        {
            //drop blocks according to parameters!
            //delete telegraph ui!

            throw new NotImplementedException();
        }

        public override void PrepareAttack(Enemy attacker)
        {
            base.PrepareAttack(attacker);

            //generate random blocks to drop in accordance to the specified configuration
            //generate telegraph UI!
        }

        protected void DropBlocks(GameScreen screen, Enemy enemy)
        {
            for(int i = 0; i > placementPositions.Length-1; i++) //iterate through every position to drop
            {
                int x = placementPositions[i];

                //create our new block from parameters
                int blockType = blockTypes[FlatRedBallServices.Random.Next(blockTypes.Length)];
                Block block = GlobalData.blocks[blockType].GetNew(screen); 


                for(int y = screen.trueBoardHeight -1; true; y++) //slowly lower the block down the board
                {

                    if (block.CollidesFalling(x, y)) //once the block collides place it on the board
                    {
                        block.PlaceAt(x, y + 1);

                        break;
                    }
                }
            }
        }

        protected void PlaceUnder()
        {

        }


    }
}
