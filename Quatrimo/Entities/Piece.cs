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
using FlatRedBall.Math;
using Quatrimo.Entities.block;
using Quatrimo.Main;
using Quatrimo.Entities.board;
using Quatrimo.Screens;
using Quatrimo.Data;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace Quatrimo.Entities
{
    public partial class Piece
    {
        protected GameScreen screen;

        public BagBlock[] bagBlocks;
        public int textureLeft;
        public int textureTop;

        int width, height;
        public int previewXOffset, previewYOffset;

        int slamOffset;

        public HsvColor hsvColor;
        public string name;

        public Piece InitializeFields(BagBlock[] bagBlocks, int width, int height, int previewXOffset, int previewYOffset, int textureLeft, int textureTop, HsvColor color, string name)
        {
            this.bagBlocks = bagBlocks;
            this.width = width;
            this.height = height;
            this.previewXOffset = previewXOffset;
            this.previewYOffset = previewYOffset;
            this.textureLeft = textureLeft;
            this.textureTop = textureTop;
            this.hsvColor = color;
            this.name = name;
            return this;
        }

        public static Piece CreateNew()
        {
            return Factories.PieceFactory.CreateNew();
        }

        public void Play(GameScreen screen)
        {
            this.screen = screen;
            boardX = screen.boardWidth/2;
            boardY = screen.visualBoardHeight-2; //set start position of piece
            
            foreach (var bagBlock in bagBlocks) //create blocks from bagblocks
            {
                Block newBlock = bagBlock.data.GetNew(screen, this, bagBlock);
                newBlock.AttachTo(this);
                newBlock.MoveToLayer(LayerProvidedByContainer);
                Blocks.Add(newBlock);

                newBlock.Play();
            }

            UpdatePos();
        }

        public void Place()
        {
            //place every block and empty local blockList
            foreach (var block in Blocks)
            {
                block.Place();
            }
            Blocks.Clear();
        }

        public void Slam()
        {
            MoveByOffset(0, -slamOffset);
        }

        /// <summary>
        /// Moves the piece to the specified board position
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(int x, int y)
        {
            boardX = x; boardY = y;
            UpdatePos();
        }

        /// <summary>
        /// Moves the piece by offsetting it from its current position
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveByOffset(int x, int y)
        {
            int adjustedX = boardX + x, adjustedY = boardY + y;
            MoveTo(adjustedX, adjustedY);
        }


        public void AttemptMove(int x, int y)
        {
            if (!Collides(x, y))
            {
                MoveByOffset(x, y);
            }
        }

        

        public void Rotate(int direction)
        {
            foreach(var block in Blocks)
            {
                block.RotateBlock(direction);
            }
            UpdatePos();
        }

        public void AttemptRotation(int direction)
        {
            if(direction != -1 && direction != 1)
            {
                throw new ArgumentException("Invalid rotation value. Expected 1 or -1, instead received: " + direction);
            }
            foreach(var block in Blocks)
            {
                if (block.CollidesRotated(direction))
                {
                    return;
                }
            }

            Rotate(direction);
        }


        /// <summary>
        /// If the piece offset by specified position will collide with anything
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        public bool Collides(int xOffset, int yOffset)
        {
            foreach(var block in Blocks)
            {
                if(block.CollidesFallingOffset(xOffset, yOffset))
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdatePos()
        {
            slamOffset = 100;
            foreach(var block in Blocks)
            {
                block.UpdateFallingPosition();

                for (int yOffset = 1; yOffset < screen.trueBoardHeight; yOffset++)
                {
                    if (block.CollidesFallingOffset(0, -yOffset))
                    {
                        slamOffset = Math.Min(slamOffset, yOffset - 1);
                        break;
                    }
                }
            }

            foreach(var block in Blocks)
            {
                block.UpdateSlamPos(-slamOffset);
            }

        }

        public void SetColor(HsvColor color)
        {
            hsvColor = color;
            foreach(var block in bagBlocks)
            {
                block.hsvColor = color;
            }

        }

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
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
