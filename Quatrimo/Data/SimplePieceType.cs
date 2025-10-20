using FlatRedBall;
using Microsoft.Xna.Framework;
using Quatrimo.Entities;
using Quatrimo.Main;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Data
{
    public class SimplePieceType : PieceType
    {
        int pieceType;
        int blockType;

        

        public SimplePieceType(PieceShape shape, int pieceType, int blockType, int textureLeft = 0, int textureTop = 30, String name = null, HsvColor[] color = null) : base(shape, textureLeft, textureTop, name, color)
        {
            this.pieceType = pieceType;
            this.blockType = blockType;
        }

        public override Piece GetPiece()
        {
            setColor();
            BagBlock[] blocks = createBlocks(blockType);
            Piece piece = Factories.PieceFactory.CreateNew();

            return piece.InitializeFields(blocks, shape.width, shape.height, shape.previewXOffset, shape.previewYOffset,textureLeft, textureTop, chosenColor, name);
        }
    }
}

