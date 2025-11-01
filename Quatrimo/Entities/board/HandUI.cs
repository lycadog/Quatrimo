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
using System.Reflection;

namespace Quatrimo.Entities.board
{
    public partial class HandUI
    {
        PieceCard activeCard;
        public void SetPosition(int boardWidth)
        {
            X = -53 - boardWidth * 5;
        }

        public void AddCard(PieceCard card)
        {
            Cards.Add(card);
            card.AttachTo(this);
            card.MoveToLayer(LayerProvidedByContainer);
        }

        public void SelectCard(int index)
        {
            if(activeCard != null)
            {
                //If there is currently a card selected, deselect it first to prevent weirdness
                DeselectCard();
            }
            NonselectedCardDimSprite.Visible = true;
            Cards[index].SelectCard();
            activeCard = Cards[index];
        }
        
        public void DeselectCard()
        {
            NonselectedCardDimSprite.Visible = false;
            if (activeCard == null)
            {
                return;
            }
            activeCard.DeselectCard();
            activeCard = null;
        }

        public void DiscardCard(int index)
        {
            Cards[index].Destroy();
            UpdateHand();
        }

        public void UpdateHand()
        {
            for(int i = 0; i < Cards.Count; i++)
            {
                Cards[i].Update(i);
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
