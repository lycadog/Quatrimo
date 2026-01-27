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
using Quatrimo.Entities.board;
using Quatrimo.Data;
using Quatrimo.Screens;
using System.Diagnostics;

namespace Quatrimo.Entities.block
{
    public partial class Block
    {
        protected GameScreen screen;

        public Piece piece;
        public bool justPlaced = false;
        public bool scored = false;
        public bool ticked = false;

        public int rotation = 0;

        /// <summary>
        /// Score awarded
        /// </summary>
        public double score = 1;
        public double times = 0;

        public Block CreateBlock(GameScreen screen, Piece piece, int localX, int localY, float textureX, float textureY, HsvColor hsvColor)
        {
            this.screen = screen;
            this.piece = piece;
            this.localX = localX;
            this.localY = localY;

            SetVisuals(textureX, textureY, hsvColor);

            return this;
        }

        public Block CreateBlock(GameScreen screen, Piece piece, int localX, int localY, float textureX, float textureY)
        {
            return CreateBlock(screen, piece, localX, localY, textureX, textureY, HsvColor.GetRandomBlockColor());
        }

        public Block CreateBlock(GameScreen screen, float textureX, float textureY, HsvColor hsvColor)
        {
            this.screen = screen;

            SetVisuals(textureX, textureY, hsvColor);

            return this;
        }

        public Block CreateBlock(GameScreen screen, float textureX, float textureY)
        {
            return CreateBlock(screen, textureX, textureY, HsvColor.GetRandomBlockColor());
        }

        void SetVisuals(float textureX, float textureY, HsvColor color)
        {
            if (!OverridesTexture)
            {
                Layer1LeftTexture = textureX;
                Layer1TopTexture = textureY;

                Layer2LeftTexture = 0;
                Layer2TopTexture = 0;
                Layer3LeftTexture = 0;
                Layer3TopTexture = 0;
            }

            SpriteLayer1.Visible = DefaultLayer1Visibility;
            SpriteLayer2.Visible = DefaultLayer2Visibility;
            SpriteLayer3.Visible = DefaultLayer3Visibility;
            updateColor(color);
        }
        
        // [----==================================================================================================----]
        //                                          -- Event Methods --
        // [----==================================================================================================----]

        public virtual void Play()
        {

        }

        public virtual void Place()
        {
            Detach();
            justPlaced = true;

            screen.AttachBlockToBoard(this);
            screen.placedBlocks.Add(this);
            screen.boardUpdated = true;
            UpdatePlacedBlockPos();

            Block placedBlock = screen.blockboard[boardX, boardY];
            placedBlock.RemovePlaced(true);
            

            //i cant figure out clipping shit do it later i guess
            screen.blockboard[boardX, boardY] = this; //TODO: DO clipping stuff
            SlamPreview1.Visible = false;
            SlamPreview2.Visible = false;
        }

        public void PlaceAt(int x, int y)
        {
            boardX = x; boardY = y;
            Place();
        }

        public virtual void Score(bool forcedRemoval = false)
        {
            scored = true;
            HideSprites();
        }

        public virtual void RemoveFalling()
        {
            piece.RemoveBlock(this);
            Destroy();
        }

        public virtual void RemovePlaced(bool forced = false)
        {
            screen.boardUpdated = true;
            screen.SetEmpty(boardX, boardY);
            Destroy();
        }

        public virtual void RemoveAndLower(bool forced = false)
        {
            screen.boardUpdated = true;
            LowerCollumn();
            Destroy();
        }

        /// <summary>
        /// Lower blocks above to fill in empty space
        /// </summary>
        /// <param name="screen"></param>
        protected void LowerCollumn()
        {
            screen.RowUpdated[boardY] = true;

            for (int y = boardY+1; y < screen.trueBoardHeight; y++) //go up and move each block down
            {
                screen.RowUpdated[y] = true;
                screen.blockboard[boardX, y].MoveTo(boardX, y - 1);

                //Create a new empty to fill in space created by lowering blocks at the top of the board
                if (y == screen.trueBoardHeight - 1)
                {
                    screen.SetEmpty(boardX, y);
                }
            }
            
        }

        public void Tick()
        {
            ticked = true;
            justPlaced = false;
            CustomTick();
        }

        protected virtual void CustomTick()
        {
            
        }

        

        // [----==================================================================================================----]
        //                                          -- Graphics Methods --
        // [----==================================================================================================----]



        public void UnhideSprites()
        {
            SpriteLayer1.Visible = DefaultLayer1Visibility;
            SpriteLayer2.Visible = DefaultLayer2Visibility;
            SpriteLayer3.Visible = DefaultLayer3Visibility;
        }
        public void HideSprites()
        {
            SpriteLayer1.Visible = false;
            SpriteLayer2.Visible = false;
            SpriteLayer3.Visible = false;
        }

        void updateColor(HsvColor hsvColor)
        {
            SpriteLayer1.Color = hsvColor.color;
            int hueOffset1 = 15;
            int hueOffset2 = 5;

            if (hsvColor.H < 210 || hsvColor.H > 15) { hueOffset1 = -hueOffset1; hueOffset2 = -hueOffset2; }

            SpriteLayer2.Color = hsvColor.getAlteredColor(hueOffset1, -0.03, - 0.20);
            SpriteLayer3.Color = hsvColor.getAlteredColor(hueOffset2, -0.06, -0.30);
        }



        // [----==================================================================================================----]
        //                                          -- Positional Methods --
        // [----==================================================================================================----]

        /// <summary>
        /// Move placed block to specified position, DOES NOT fill in spot left behind !!!!! it must be filled in seperately!
        /// </summary>
        /// <param name="board"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(int x, int y)
        {
            screen.boardUpdated = true;
            screen.blockboard[x, y] = this;
            boardX = x; boardY = y;
            UpdatePlacedBlockPos();
        }

        /// <summary>
        /// Check if the block moved to the specified position will collide with anything
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CollidesFalling(int x, int y)
        {
            if (screen.IsOutsideBounds(x, y)) { return true; }

            return CollidesWhenFalling && screen.blockboard[x, y].CollidesWhenPlaced;
        }

        public bool CollidesFallingOffset(int xOffset, int yOffset)
        {
            return CollidesFalling(boardX + xOffset, boardY + yOffset);
        }

        public void RotateBlock(int direction)
        {
            (int, int) newPos = GetRotatePos(direction);
            localX = newPos.Item1; localY = newPos.Item2;
            if (RotateSprite)
            {
                //figure this out later TODO
            }
        }

        /// <summary>
        /// Returns if the block rotated in specified direction will collide with anything
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool CollidesRotated(int direction)
        {
            (int, int) newPos = GetRotatePos(direction);
            int x = newPos.Item1; int y = newPos.Item2;
            x = x + piece.boardX; y = y + piece.boardY;
            return CollidesFalling(x, y);
        }

        protected (int, int) GetRotatePos(int direction)
        {
            return (localY * -direction, localX * direction);
        }

        /// <summary>
        /// Updates the board and slam position of this falling block using its localpos and the position of the piece
        /// </summary>
        public void UpdateFallingPosition()
        {
            boardX = piece.boardX + localX;
            boardY = piece.boardY + localY;

            RelativeX = localX * 10; RelativeY = localY * 10;

            UpdatePos();
        }

        public void UpdateSlamPos(int slamOffset)
        {
            SlamPreview1.RelativeY = slamOffset * 10;
            SlamPreview2.RelativeY = slamOffset * 10;
        }

        /// <summary>
        /// Updates placed block positions
        /// </summary>
        public void UpdatePlacedBlockPos()
        {
            RelativeX = boardX * 10 + 10; RelativeY = boardY * 10 + 10;
            UpdatePos();
        }

        protected void UpdatePos()
        {
            if (boardY >= screen.visualBoardHeight) //hide block if it extends above the board into the buffer
            {
                HideSprites();
            }
            else
            {
                UnhideSprites();
            }
        }

        // [----==================================================================================================----]
        //                                              -- Engine Methods --
        // [----==================================================================================================----]

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
            
        }

        private void CustomDestroy()
        {
            screen.placedBlocks.Remove(this);
            Detach();
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
