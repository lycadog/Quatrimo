using FlatRedBall;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Instructions;
using FlatRedBall.Screens;
using Gum.Converters;
using Microsoft.Xna.Framework.Graphics;
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
        public int maxBlocksDropped = 16;

        public bool depthRestricted = true; // Whether or not the depth values are used to restrict block depth
        public bool depthEnforced = false; // Whether or not all blocks dropped match the depth perfectly (so 2 depth = 2 blocks dropped on every spot)

        //Range of allowed depth
        public int depthMin = 2;
        public int depthMax = 3;

        //Misc toggles and values
        public bool clustered = false; //Places blocks near 1-2 dense clusters. More likely to lead to high depth
        public bool symmetrical = false; //Places blocks symmetrically, enforces even number of blocks

        //block visuals
        public int textureX = 0;
        public int textureY = 50;
        public HsvColor color;
        public bool useRandomColor = true; //generates a random color for every block?
        public bool generateColorOnPrepare = true; //generate a new color every time the attack is prepared? if no, will generate on instantiation

        public PlacementType placementType = PlacementType.DroppedFromAbove;

        // =================== Active Fields ===================

        protected int blockCount;
        protected int depth;

        protected QueuedBlock[,] BlockArray;

        protected List<QueuedBlock> QueuedBlocks = [];
        protected List<Sprite> TelegraphSprites = [];

        public enum PlacementType
        {
            DroppedFromAbove,
            RaisedFromBelow,
        }

        protected class QueuedBlock : IEquatable<QueuedBlock>, IComparable<QueuedBlock>
        {
            public Block block;
            public int x, y;

            public int finalY;
            public int type;
            public bool placed = false;

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


        public override void PrepareAttack(GameScreen screen, Enemy attacker)
        {
            base.PrepareAttack(screen, attacker);


            QueuedBlocks.Clear(); //clear out values from previous attacks


            //todo: add symmetrical attack support

            // ==================== set all the values needed for the blocks ====================

            //Set random values
            blockCount = FlatRedBallServices.Random.Next(minBlocksDropped, maxBlocksDropped);
            depth = FlatRedBallServices.Random.Next(depthMin, depthMax);

            if (generateColorOnPrepare && useRandomColor)
            {
                color = HsvColor.GetRandomBlockColor();
            }


            int realArrayHeight = 1; //We will use this later to recreate the array from the real height.

            int arrayDepth = screen.visualBoardHeight;
            if (depthRestricted) { arrayDepth = depth; realArrayHeight = depth; } //If depth restricted, restrict the array height

            QueuedBlock[,] tempBlockArray = new QueuedBlock[screen.boardWidth, arrayDepth];

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
                    if (tempBlockArray[x, y] == null)
                    {
                        tempBlockArray[x, y] = block;
                        block.y = y;

                        realArrayHeight = Math.Max(y, realArrayHeight); //get real height to be used by the non-temporary array

                        break;

                        //add block to our lists to be used later

                    }
                }
            }

            if (depthEnforced) //go through the 2d array and add blocks ontop of others to match the depth
            {
                for (int x = 0; x < tempBlockArray.GetLength(0); x++)
                {
                    if (tempBlockArray[x, 0] != null)
                    {
                        //TODO: fill everything in with blocks here
                    }
                }
            }

            //Copy the temporary array into a shorter more accurately sized array, to be used later.
            BlockArray = new QueuedBlock[tempBlockArray.GetLength(0), realArrayHeight];

            for(int x = 0; x < tempBlockArray.GetLength(0); x++)
            {
                for(int y = 0; y < realArrayHeight; y++)
                {
                    BlockArray[x, y] = tempBlockArray[x, y];
                }
            }

            CreateTelegraphGraphics(screen);

            //TODO: we are looping through this arraay like 50 times. maybe regroup these a bit?
            //generate random blocks to drop in accordance to the specified configuration
            //generate telegraph UI!
        }

        public override void UpdatePreparedAttack(GameScreen screen, Enemy enemy)
        {
            foreach(var queuedBlock in QueuedBlocks)
            {
                queuedBlock.block.UpdateSlamPos(queuedBlock.y);
            }
        }

        public override void StartAttack(GameScreen screen, Enemy enemy)
        {

            //drop blocks according to parameters!
            //delete telegraph ui!
            //start animations!
            switch (placementType)
            {
                case PlacementType.DroppedFromAbove:

                    StartBlockDrop(screen, enemy);
                    break;

                case PlacementType.RaisedFromBelow:

                    PlaceUnder(screen, enemy);
                    break;

                default:

                    throw new InvalidOperationException("Attempted to execute BlockPlacement with invalid PlacementType");

            }
        }

        public override bool ResolveAttack(GameScreen screen, Enemy enemy)
        {

            switch (placementType)
            {
                case PlacementType.DroppedFromAbove:

                    bool complete = true;
                    foreach (var block in QueuedBlocks)
                    {
                        if (!CheckFallingBlock(block))
                        {
                            complete = false;
                        }
                    }

                    return complete;

                case PlacementType.RaisedFromBelow:

                    return true;

                default:

                    throw new InvalidOperationException("Attempted to run BlockPlacement UpdateAttack() with invalid PlacementType");

            }
        }

        
        /// <summary>
        /// Queue up every single block to be dropped using instructions
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="enemy"></param>
        protected void StartBlockDrop(GameScreen screen, Enemy enemy)
        {
            int blockdrops = 0;

            foreach(var sprite in TelegraphSprites)
            {
                SpriteManager.RemoveSprite(sprite);
            }

            //use delegate instructions to delay dropping each block based on its X position
            //genius!
            foreach (var blockToDrop in QueuedBlocks)
            {
                blockdrops++;


                Debug.WriteLine($"dropped block {blockToDrop.x}, {blockToDrop.y}");

                InstructionManager.Add(new MethodInstruction<BlockPlacementAttack>(
                    this,
                    "DropBlock",
                    [screen, blockToDrop],
                    TimeManager.CurrentTime + 0.03f * blockToDrop.x));
            }

            Debug.WriteLine($"queued blocks length: {QueuedBlocks.Count}, blocks dropped: {blockdrops}");
        }

        /// <summary>
        /// Trace path downwards and begin moving the block
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="queuedBlock"></param>
        protected void DropBlock(GameScreen screen, QueuedBlock queuedBlock)
        {
            int x = queuedBlock.x;

            Block block = queuedBlock.block;

            block.HideSlamIndicator();

            for (int y = screen.trueBoardHeight - 1; true; y--) //slowly lower the block down the board
            {
                if (block.CollidesFalling(x, y)) //once the block collides place it on the board above the block it collided with
                {
                    queuedBlock.finalY = y + 1 + queuedBlock.y; //y + 1 because y is the block that we collided with, so we need to place ontop of it
                    //we need to add the queued block Y to avoid conflicts with multiple blocks dropped at once on the same X position

                    block.RelativeYAcceleration = -500f;

                    //SpriteManager.RemoveSprite(queuedBlock.telegraphSprite);

                    break;
                }
            }
        }

        /// <summary>
        /// Call on a falling block to check if it has fallen into place yet.
        /// </summary>
        /// <param name="queuedblock"></param>
        /// <returns></returns>
        protected static bool CheckFallingBlock(QueuedBlock queuedblock)
        {
            if (queuedblock.placed)
            {
                return true;
            }

            Block block = queuedblock.block;

            //check if the block has reached its final position
            if (block.RelativeY <= queuedblock.finalY * 10 + 10)
            {
                //place the block and stop animating it
                block.PlaceAt(queuedblock.x, queuedblock.finalY);
                block.RelativeYAcceleration = 0;
                block.RelativeYVelocity = 0;
                queuedblock.placed = true;
                return true;
            }


            //RelativeX = boardX * 10 + 10; RelativeY = boardY * 10 + 10;

            return false;
        }

        /// <summary>
        /// Place block under the board
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="enemy"></param>
        protected void PlaceUnder(GameScreen screen, Enemy enemy)
        {
            //code to raise a collumn, then insert new block, similar to lower collumn!

            foreach (var queuedBlock in QueuedBlocks)
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

        /// <summary>
        /// Create sprites and set up everything to show the attack's telegraph
        /// </summary>
        /// <param name="screen"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected void CreateTelegraphGraphics(GameScreen screen)
        {
            switch (placementType)
            {
                case PlacementType.DroppedFromAbove:

                    CreateDroppedTelegraphUI(screen);
                    break;

                case PlacementType.RaisedFromBelow:

                    break;

                default:

                    throw new InvalidOperationException("Attempted to execute BlockPlacement with invalid PlacementType");

            }
        }


        protected void CreateDroppedTelegraphUI(GameScreen screen)
        {

            Debug.WriteLine($"block array size: {BlockArray.GetLength(0)}, {BlockArray.GetLength(1)}");

            for(int x = 0; x < BlockArray.GetLength(0); x++)
            {

                Sprite arrowSprite = SpriteManager.AddSprite(atlas, screen.FalingBlocksLayer);

                for (int y = 0; y < BlockArray.GetLength(1); y++)
                {

                    Debug.WriteLine($"block {x},{y} is null? {BlockArray[x,y] == null}");

                    if (BlockArray[x,y] == null)
                    {
                        if(y == 0) { SpriteManager.RemoveSprite(arrowSprite); } //so fucking jank

                        break;
                    }

                    QueuedBlock queuedBlock = BlockArray[x, y];

                    Block block = GlobalData.blocks[queuedBlock.type].GetNew(screen);
                    block.CreateBlock(screen, textureX, textureY, color);

                    queuedBlock.block = block;

                    screen.AttachBlockToBoard(block, false);
                    block.UnhideSprites();
                    block.RelativeX = queuedBlock.x * 10 + 10;
                    block.RelativeY = screen.visualBoardHeight * 10;

                    block.boardX = queuedBlock.x;
                    block.boardY = screen.visualBoardHeight;

                    block.SetEnemyBlock();
                    block.UpdateSlamPos(y);

                    QueuedBlocks.Add(queuedBlock);

                    if (y == 0)
                    {
                        //set sprite position and texture and stuff
                        arrowSprite.LeftTexturePixel = 30;
                        arrowSprite.RightTexturePixel = 40;
                        arrowSprite.TopTexturePixel = 130;
                        arrowSprite.BottomTexturePixel = 140;

                        arrowSprite.Width = 10;
                        arrowSprite.Height = 10;

                        TelegraphSprites.Add(arrowSprite);

                        screen.AttachToBoard(arrowSprite);
                        arrowSprite.RelativeX = x * 10 + 10;
                        arrowSprite.RelativeY = screen.visualBoardHeight * 10 - 10;

                        arrowSprite.Visible = true;
                    }


                    if (y > 0) //we need to move over blocks on the same X position out of the way or we'll have bugs
                    {
                        for(int loweredY = y - 1; loweredY > -1; loweredY--)
                        {

                            BlockArray[x, loweredY].block.RelativeY -= 10;
                            BlockArray[x, loweredY].block.boardY--;

                            BlockArray[x, loweredY].block.UpdateSlamPos(BlockArray[x, loweredY].y);
                        }
                        arrowSprite.RelativeY -= 10;
                    }
                }
                //we will properly create our blocks here and attach their sprites to the board at the top, so they will be ready to fall.
                //when another block is detected at the same X value, we need to go to the previous one and move it.
            }
        }


        public override void HideTelegraphSprites()
        {
            foreach (var block in QueuedBlocks)
            {
                block.block.Alpha = 0.2f;
                block.block.SlamPreviewAlpha = 0.5f;
            }

            foreach(var sprite in TelegraphSprites)
            {
                sprite.Alpha = 0.2f;
            }

            //hideother telegraph stuff here too!
        }

        public override void UnhideTelegraphSprites()
        {
            foreach (var block in QueuedBlocks)
            {
                block.block.Alpha = 1;
                block.block.SlamPreviewAlpha = 0.75f;
            }

            foreach (var sprite in TelegraphSprites)
            {
                sprite.Alpha = 1;
            }
        }
    }
}
