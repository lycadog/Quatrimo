using FlatRedBall;
using Gum.Converters;
using Quatrimo.Data;
using Quatrimo.Entities.block;
using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class BlockPlacementAttack : EnemyAttack
    {

        /// =================== Configuration fields ===================
        public int[] blockTypes = [0];

        // Range of blocks dropped
        public int minBlocksDropped = 4;
        public int maxBlocksDropped = 18;

        public bool depthRestricted = true; // Whether or not the depth values are used to restrict block depth
        public bool depthEnforced = false; // Whether or not all blocks dropped match the depth perfectly (so 2 depth = 2 blocks dropped on every spot)

        //Range of allowed depth
        public int depthMin = 1;
        public int depthMax = 2;

        //Misc toggles and values
        public bool clustered = false; //Places blocks near 1-2 dense clusters. More likely to lead to high depth
        public bool symmetrical = false; //Places blocks symmetrically, enforces even number of blocks

        //block visuals
        public int textureX = 120;
        public int textureY = 30;
        public HsvColor color;
        public bool useRandomColor = true; //generates a random color for every block?
        public bool generateColorOnPrepare = true; //generate a new color every time the attack is prepared? if no, will generate on instantiation

        public PlacementType placementType = PlacementType.DroppedFromAbove;

        // =================== Active Fields ===================

        protected int blockCount;
        protected int depth;
        protected List<QueuedBlock> BlockList = [];

        public enum PlacementType
        {
            DroppedFromAbove,
            RaisedFromBelow,
        }

        protected class QueuedBlock : IEquatable<QueuedBlock>, IComparable<QueuedBlock>
        {
            public int x, y;
            public int type;

            public QueuedBlock(int x, int type)
            {
                this.x = x;
                this.type = type;
            }

            public int CompareTo(QueuedBlock compareBlock)
            {
                if (compareBlock == null)
                    return 1;
                else
                    return this.x.CompareTo(compareBlock.x);
            }

            public bool Equals(QueuedBlock other)
            {
                if (other == null) return false;
                return (this.x.Equals(other.x));
            }
        }

        public BlockPlacementAttack()
        {
            if (useRandomColor)
            {
                color = HsvColor.GetRandomBlockColor();
            }
        }

        // =================== Attack Methods ===================

        public override void ExecuteAttack(GameScreen screen, Enemy enemy)
        {
            //drop blocks according to parameters!
            //delete telegraph ui!
            //start animations!
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

        public override void PrepareAttack(GameScreen screen, Enemy attacker)
        {
            base.PrepareAttack(screen, attacker);

            //todo: add symmetrical attack support

            // ==================== set all the values needed for the blocks ====================

            //Set random values
            blockCount = FlatRedBallServices.Random.Next(minBlocksDropped, maxBlocksDropped);
            depth = FlatRedBallServices.Random.Next(depthMin, depthMax);
            if (generateColorOnPrepare && useRandomColor)
            {
                color = HsvColor.GetRandomBlockColor();
            }

            int arrayDepth = screen.trueBoardHeight;
            if (depthRestricted) { arrayDepth = depth; } //If depth restricted, restrict the array height
            QueuedBlock[,] blockArray = new QueuedBlock[screen.boardWidth, arrayDepth];

            int maxClusters = 3;
            if (blockCount < 10) //lower cluster count if low block count
            {
                maxClusters = 2;
            }

            int[] clusterCenters = new int[FlatRedBallServices.Random.Next(1, maxClusters + 1)];

            if (clustered) //Generate random cluster centers if attack is clustered
            {
                for (int i = 0; i < clusterCenters.Length; i++)
                {
                    clusterCenters[i] = FlatRedBallServices.Random.Next(screen.boardWidth);
                }
            }


            // ==================== start creating the queued blocks and arraying them ====================

            for (int i = 0; i < blockCount; i++) //make new blocks until we've made enough to reach blockCount
            {

                int x;
                if (clustered) //if clustered, choose a cluster. then, set X near that cluster
                {
                    int center = clusterCenters[FlatRedBallServices.Random.Next(clusterCenters.Length)]; //get random cluster to place around

                    x = center + FlatRedBallServices.Random.Next(-2, 3);
                    x = Math.Clamp(x, 0, screen.boardWidth - 1);
                }
                else
                {
                    x = FlatRedBallServices.Random.Next(screen.boardWidth); //if not clustered, get X value normally
                }

                int type = blockTypes[FlatRedBallServices.Random.Next(blockTypes.Length)];

                QueuedBlock block = new QueuedBlock(x, type);

                for (int y = 0; y < arrayDepth; y++)
                {
                    //find the next empty spot in the array. if there are no empty blocks, this block is discarded by not reaching the if statement below
                    if (blockArray[x, y] == null)
                    {
                        blockArray[x, y] = block;
                        BlockList.Add(block);
                        //add block to our lists to be used later
                    }
                }
            }

            if (depthEnforced) //go through the 2d array and add blocks ontop of others to match the depth
            {
                for (int x = 0; x < blockArray.GetLength(0); x++)
                {
                    if (blockArray[x, 0] != null)
                    {
                        //TODO: fill everything in with blocks here
                    }
                }
            }

            BlockList.Sort();


            //TODO: add telegraping here




            //generate random blocks to drop in accordance to the specified configuration
            //generate telegraph UI!
        }

        protected void DropBlocks(GameScreen screen, Enemy enemy)
        {

            foreach (var blockToDrop in BlockList)
            {
                int x = blockToDrop.x;

                //create our new block from parameters
                Block block = GlobalData.blocks[blockToDrop.type].GetNew(screen);
                block.CreateBlock(screen, textureX, textureY, color);

                for (int y = screen.trueBoardHeight - 1; true; y--) //slowly lower the block down the board
                {
                    if (block.CollidesFalling(x, y)) //once the block collides place it on the board above the block it collided with
                    {
                        block.PlaceAt(x, y + 1); //hence the y + 1

                        break;
                    }
                }

            }
        }

        protected void PlaceUnder(GameScreen screen, Enemy enemy)
        {
            //code to raise a collumn, then insert new block, similar to lower collumn!

            foreach (var queuedBlock in BlockList)
            {
                Debug.WriteLine("attack block X: " + queuedBlock.x);
                int x = queuedBlock.x;

                //create our new block from parameters
                Block block = GlobalData.blocks[queuedBlock.type].GetNew(screen);
                block.CreateBlock(screen, textureX, textureY, color);

                //remove block at the top of the board so it is not raised into the void.
                screen[x, screen.trueBoardHeight - 1].Destroy();
                //sorry block!

                //not working for some reason, adding an extra block of air right above the newly added block

                for (int y = screen.trueBoardHeight - 2; y >= 0; y--) //iterate top to bottom through the board, raising every block by one
                {
                    screen[x, y].MoveTo(x, y + 1); //move block up one!
                }

                block.PlaceAt(x, 0, true); //fill in the empty spot at the bottom!
            }
        }



    }
}
