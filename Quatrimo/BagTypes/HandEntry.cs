using Quatrimo.Entities;
using Quatrimo.GumRuntimes;
using Quatrimo.GumRuntimes.Quatrimo;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.BagTypes
{
    public class HandEntry
    {

        public Piece piece;
        public PieceCardRuntime card;

        GameScreen screen;

        public HandEntry(Piece piece, GameScreen screen)
        {
            this.piece = piece;
            this.screen = screen;

            card = new PieceCardRuntime();
            //tie event to info button

            //create all the sprites required for block display
            //create piece sprites


        }

        public void Select()
        {
            card.CurrentCardCategoryState = PieceCardRuntime.CardCategory.Selected;
            card.MoveToFrbLayer(screen.LowUILayer, screen.GetGumLayer(screen.MainUILayer));
            //move layer so highlight border works
            
        }

        public void Unselect()
        {
            card.CurrentCardCategoryState = PieceCardRuntime.CardCategory.Default;
            card.MoveToFrbLayer(screen.LowUILayer, screen.GetGumLayer(screen.LowUILayer));
        }

        public void Hide()
        {
            card.CurrentCardCategoryState = PieceCardRuntime.CardCategory.Unselected;
            card.MoveToFrbLayer(screen.LowUILayer, screen.GetGumLayer(screen.LowUILayer));
        }

        public void Update(int index)
        {
            card.Y = 35 * index;
            SetNumber(index);
        }

        void SetNumber(int number)
        {
            if(number > 10)
            {
                card.CurrentNumberState = PieceCardRuntime.Number.NoNumber;
                return;
            }
            card.CurrentNumberState = (PieceCardRuntime.Number?)number;
        }

    }
}
