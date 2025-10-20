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
using Quatrimo.Screens;

namespace Quatrimo.Entities
{
    public partial class PieceCard
    {
        int index;

        //number one = 70, 40

        public void Initialize(Piece piece)
        {
            
            foreach (var block in piece.bagBlocks)
            {
                AttachPreviewSprite(piece, block);
            }

            CardBorder.Color = piece.hsvColor.color;

            PieceTypeBox.LeftTexturePixel = piece.CardTypeTextureLeft;
            PieceTypeBox.RightTexturePixel = piece.CardTypeTextureLeft + 7;

            PieceTypeBox.TopTexturePixel = piece.CardTypeTextureTop;
            PieceTypeBox.BottomTexturePixel = piece.CardTypeTextureTop + 7;

            Number.TopTexturePixel = 40; Number.BottomTexturePixel = 50;
        }


        public void AddToHand(GameScreen screen, Piece piece, int index)
        {
            Initialize(piece);
            
            Update(index);

            piece.RelativeX = screen.boardWidth / 2 * 10 + 35;
        }

        public void Update(int index)
        {
            RelativeX = 3;
            RelativeY = 95.5f - index * 40;

            this.index = index;
            if(index < 10)
            {
                Number.Visible = true;
                int leftPixel = 70 + index * 10;
                Number.LeftTexturePixel = leftPixel; Number.RightTexturePixel = leftPixel + 10;
            }
            else { Number.Visible = false; }
    
        }

        public void AttachPreviewSprite(Piece piece, BagBlock block)
        {
            Sprite sprite = block.data.CreatePreview(block, LayerProvidedByContainer);

            sprite.AttachTo(this);
            sprite.RelativeX = -7.5f + block.localX * 5 + piece.previewXOffset * 5;
            sprite.RelativeY = 2.5f + block.localY * 5 + piece.previewYOffset * 5;
            sprite.RelativeZ = 1;
            
            

            PreviewSprites.Add(sprite);
        }

        public void SelectCard()
        {
            RelativeZ = 3;

            SelectedBarTop.Visible = true;
            SelectedBarBottom.Visible = true;
            
        }

        public void DeselectCard()
        {
            RelativeZ = 0;

            SelectedBarTop.Visible = false;
            SelectedBarBottom.Visible = false;
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
            foreach (var sprite in PreviewSprites)
            {
                SpriteManager.RemoveSprite(sprite);
            }
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
