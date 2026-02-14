using FlatRedBall;
using Gum.Converters;
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
        public bool depthEnforced = false; // Whether or not all blocks dropped match the depth perfectly

        //Range of allowed depth
        public int depthMin = 1;
        public int depthMax = 2;

        //Misc toggles and values
        public bool clustered; //Places blocks near dense clusters, more likely to lead to high depth
        public bool symmetrical; //Places blocks symmetrically, enforces even number of blocks
        public bool randomizePositions = true;
        public float leftRightBias = 0.5f; //bias towards left or right side. 0.5 for even


        //block visuals
        public int textureX;
        public int textureY;
        public HsvColor color;
        public bool useRandomColor = true;
        
        public enum PlacementType
        {
            DroppedFromAbove,
            RaisedFromBelow,
        }

        PlacementType placementType;

        public override void ExecuteAttack(GameScreen screen, Enemy enemy)
        {
            //drop blocks according to parameters!
            //delete telegraph ui!
            switch (placementType)
            {
                case PlacementType.DroppedFromAbove:

                    DropBlocks(screen, enemy);
                    break;

                case PlacementType.RaisedFromBelow:

                    PlaceUnder(screen, enemy);
                    break;

                default:

                    throw new InvalidOperationException("Attempted to execute BlockPlacement with invalid PlacementType");
                    
            }
        }

        public override void PrepareAttack(Enemy attacker)
        {
            base.PrepareAttack(attacker);
            if (useRandomColor)
            {
                color = HsvColor.GetRandomBlockColor();
            }

            //generate random blocks to drop in accordance to the specified configuration
            //generate telegraph UI!
        }

        protected void DropBlocks(GameScreen screen, Enemy enemy)
        {
            for(int i = 0; i < placementPositions.Length; i++) //iterate through every position to drop
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

        protected void PlaceUnder(GameScreen screen, Enemy enemy)
        {
            //code to raise a collumn, then insert new block, similar to lower collumn!

            for(int i = 0; i < placementPositions.Length; i++) //iterate through every attack position
            {
                int x = placementPositions[i];

                int blockType = blockTypes[FlatRedBallServices.Random.Next(blockTypes.Length)]; //create our block
                Block block = GlobalData.blocks[blockType].GetNew(screen);

                //remove block at the top of the board so it is not raised into the void.
                screen[x, screen.trueBoardHeight - 1].Destroy();
                //sorry block!


                for (int y = screen.trueBoardHeight - 2; y > -1; y--) //iterate top to bottom through the board, raising every block by one
                {
                    screen[x, y].MoveTo(x, y + 1); //move block up one!
                }

                block.PlaceAt(x, 0); //fill in the empty spot at the bottom!
            }

        }




    }
}
