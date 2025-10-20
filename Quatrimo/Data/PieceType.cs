using FlatRedBall;
using Microsoft.Xna.Framework;
using Quatrimo.Entities;
using Quatrimo.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Data
{
    public abstract class PieceType
    {
        public PieceShape shape;

        public int textureLeft;
        public int textureTop;
        public HsvColor[] colors;
        public string name;
        public bool colorOverride = true; //defaults to false unless colors are given

        protected HsvColor chosenColor;

        protected PieceType(PieceShape shape, int textureLeft, int textureTop, string name = null, HsvColor[] colors = null)
        {
            this.shape = shape;
            this.textureLeft = textureLeft;
            this.textureTop = textureTop;
            this.colors = colors;
            this.name = name;
            if(colors == null) { colorOverride = false; }
            if(name == null) { name = shape.name; }
        }

        public abstract Piece GetPiece();

        public BagBlock[] createBlocks(int[] blockTypes)
        {
            BagBlock[] blocks = new BagBlock[shape.blockCount];
            int index = 0;
            for (int x = 0; x < shape.width; x++)
            {
                for (int y = 0; y < shape.height; y++)
                {
                    if (shape.shape[x, y] != 0)
                    {
                        //create new block if specified x and y position is not empty
                        //increment index in blocks array
                        int localX = x - shape.originX, localY = y - shape.originY;

                        blocks[index] = new BagBlock(blockTypes[shape.shape[x,y]], localX, localY, chosenColor);
                        index++;
                    }

                }
            }
            return blocks;
        }

        public BagBlock[] createBlocks(int blockType)
        {
            return createBlocks([blockType, blockType, blockType, blockType, blockType]);
        }

        public void setColor()
        {
            if (colorOverride)
            {
                chosenColor = colors[FlatRedBallServices.Random.Next(colors.Length)];
                return;
            }

            //pieces by default will get a color with a random hue and a small range of saturation and values
            int h = FlatRedBallServices.Random.Next(36) * 10;
            double s = 1 - FlatRedBallServices.Random.Next(3) * .10;
            double v = 1 - FlatRedBallServices.Random.Next(3) * .08;
            chosenColor = new HsvColor(h, s, v);

        }
    }
}
