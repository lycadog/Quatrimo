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
    public class PoolPieceType : PieceType
    {
        ObjPool<int> piecePool;
        ObjPool<int> blockPool;

        static int[] randomGroupPool = { 1, 2, 2, 3, 4}; //use this in random type distribution

        public PoolPieceType(PieceShape shape, ObjPool<int> piecePool, ObjPool<int> blockPool, int textureLeft, int textureTop, string name = null, HsvColor[] color = null) : base(shape, textureLeft, textureTop, name, color)
        {
            this.piecePool = piecePool;
            this.blockPool = blockPool;
        }

        //how does random type distribution work?
        //we take two random block types, then get a random number from 1 to 4 using the array above
        //this random number will be the type1Count variable
        //that many groups will be assigned to block type 1, randomly selecting any of the 4 groups
        //then, the remaining groups will be assigned to block type 2

        public override Piece GetPiece()
        {
            setColor();

            int[] blockTypes = DistributeBlockTypes();
            BagBlock[] blocks = createBlocks(blockTypes);
            Piece piece = Factories.PieceFactory.CreateNew();

            return piece.InitializeFields(blocks, shape.width, shape.height, shape.previewYOffset, shape.previewYOffset, textureLeft, textureTop, chosenColor, name);
        }

        public int[] DistributeBlockTypes()
        {
            int blocktype1 = blockPool.getRandom();
            int blocktype2 = blockPool.getRandom();
            int blocktype3 = blockPool.getRandom();

            int[] blockTypes = new int[5]; //contains block ids for each of the 5 groups
            int type1Count = randomGroupPool[FlatRedBallServices.Random.Next(5)];

            bool[] takenGroups = new bool[4]; //holds which group has been set to type1
            int i = 0;
            while (i < type1Count) //inefficient as fuuuck todo fix this
            {
                //until we have specified all the groups to type 1, keep grabbing random ones
                int index = FlatRedBallServices.Random.Next(4);
                if (takenGroups[index])
                {
                    continue;
                }
                takenGroups[index] = true;
                blockTypes[index] = blocktype1;
                i++;
            }

            for (i = 0; i < 4; i++)
            {
                if (!takenGroups[i])
                {
                    blockTypes[i] = blocktype2;
                }
            }
            blockTypes[4] = blocktype3;
            return blockTypes;
        }
    }
}
