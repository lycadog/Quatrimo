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

namespace Quatrimo.BagTypes.Bagblocks
{
    public class BombBlockData : BlockData
    {
        public override Sprite CreatePreview(BagBlock block, Layer layer)
        {
            Sprite sprite = GetDefaultSprite(block, layer);

            sprite.LeftTexturePixel = 5; sprite.RightTexturePixel = 10;
            sprite.TopTexturePixel = 40; sprite.BottomTexturePixel = 45;

            return sprite;
        }

        public override BombBlock GetNew(GameScreen screen, Piece piece, BagBlock bagBlock)
        {
            BombBlock block = Factories.BombBlockFactory.CreateNew();

            return (BombBlock)block.CreateBlock(screen, piece, bagBlock.localX, bagBlock.localY, piece.textureLeft, piece.textureTop, bagBlock.hsvColor);
        }

        public override Block GetNew(GameScreen screen)
        {
            BombBlock block = Factories.BombBlockFactory.CreateNew();

            return block;
        }
    }
}
