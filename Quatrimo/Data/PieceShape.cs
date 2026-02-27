using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Data
{
    public class PieceShape
    {
        //todo: make a bunch of basic shapes out of this! it's not gonna change after all
        public int[,] shape;
        public int originX, originY;
        public int blockCount;
        public int width, height;
        public int previewXOffset, previewYOffset;

        public string name;

        //Basic PieceType with no special types or anything
        public SimplePieceType B;

        public PieceShape(int[,] shape, int originX, int originY, int blockCount, string name = "temporary name WIP", int previewXOffset = 0, int previewYOffset = 0)
        {
            this.shape = shape;
            this.originX = originX;
            this.originY = originY;
            this.previewXOffset = previewXOffset;
            this.previewYOffset = previewYOffset;
            this.blockCount = blockCount;
            this.name = name;

            width = shape.GetLength(0);
            height = shape.GetLength(1);

            B = new(this, 0, 0);
        }

    }
}
