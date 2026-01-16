using FlatRedBall;
using FlatRedBall.Graphics;
using Quatrimo.Entities;
using Quatrimo.Entities.block;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class CursedBlockData : BlockData
    {
        public override Sprite CreatePreview(BagBlock block, Layer layer)
        {
            Sprite sprite = GetDefaultSprite(block, layer);

            sprite.LeftTexturePixel = 10; sprite.RightTexturePixel = 15;
            sprite.TopTexturePixel = 40; sprite.BottomTexturePixel = 45;

            return sprite;
        }

        public override CursedBlock GetNew(GameScreen screen, Piece piece, BagBlock bagBlock)
        {
            CursedBlock block = Factories.CursedBlockFactory.CreateNew();

            return (CursedBlock)block.CreateBlock(screen, piece, bagBlock.localX, bagBlock.localY, piece.textureLeft, piece.textureTop, bagBlock.hsvColor);
        }
    }
}
