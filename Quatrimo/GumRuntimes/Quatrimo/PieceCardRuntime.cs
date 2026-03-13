using Quatrimo.Entities;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quatrimo.GumRuntimes.Quatrimo
{
    public partial class PieceCardRuntime
    {
        public Piece piece;

        GameScreen screen;

        public PieceCardRuntime(Piece piece, int number, GameScreen screen)
        {
            this.piece = piece;
            this.screen = screen;

            SetNumber(number);
        }

        public void Select()
        {
            CurrentCardCategoryState = CardCategory.Selected;
            MoveToFrbLayer(screen.LowUILayer, screen.GetGumLayer(screen.MainUILayer));
            //move layer so highlight border works
        }

        public void Unselect()
        {
            CurrentCardCategoryState = CardCategory.Default;
            MoveToFrbLayer(screen.LowUILayer, screen.GetGumLayer(screen.LowUILayer));
        }

        public void Hide()
        {
            CurrentCardCategoryState = CardCategory.Unselected;
            MoveToFrbLayer(screen.LowUILayer, screen.GetGumLayer(screen.LowUILayer));
        }

        public void Update(int index)
        {
            SetNumber(index);
        }

        void SetNumber(int number)
        {
            if (number > 10)
            {
                CurrentNumberState = Number.NoNumber;
                return;
            }
            CurrentNumberState = (Number?)number;
        }

        partial void CustomInitialize () 
        {
        }
    }
}
