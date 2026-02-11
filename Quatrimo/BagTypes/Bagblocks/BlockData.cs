using FlatRedBall;
using FlatRedBall.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo.Entities;
using Quatrimo.Entities.block;
using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    /// <summary>
    /// Stores global data for blocks
    /// </summary>
    public class BlockData
    {
        static Texture2D cardTexture = FlatRedBallServices.Load<Texture2D>("Content/pieceCard.png");

        public virtual Sprite CreatePreview(BagBlock block, Layer layer)
        {
            Sprite sprite = GetDefaultSprite(block, layer);

            sprite.LeftTexturePixel = 0; sprite.RightTexturePixel = 5;
            sprite.TopTexturePixel = 40; sprite.BottomTexturePixel = 45;

            return sprite;
        }

        protected Sprite GetDefaultSprite(BagBlock block, Layer layer)
        {
            Sprite sprite = SpriteManager.AddSprite(cardTexture, layer);
            
            sprite.Color = block.hsvColor.color;
            sprite.ColorOperation = FlatRedBall.Graphics.ColorOperation.ColorTextureAlpha;
            sprite.Width = 5; sprite.Height = 5;
            return sprite;
        }

        /// <summary>
        /// Get initialized block
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="piece"></param>
        /// <param name="bagBlock"></param>
        /// <returns></returns>
        public virtual Block GetNew(GameScreen screen, Piece piece, BagBlock bagBlock)
        {
            Block block = Factories.BlockFactory.CreateNew();

            return block.CreateBlock(screen, piece, bagBlock.localX, bagBlock.localY, piece.textureLeft, piece.textureTop, bagBlock.hsvColor);
        }

        /// <summary>
        /// Get blank block
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public virtual Block GetNew(GameScreen screen)
        {
            Block block = Factories.BlockFactory.CreateNew();

            return block;
        }


        //TODO: methods for preview graphics, blocksense description, etc
    }
}
