using Quatrimo.Entities;
using Quatrimo.Screens;
using Quatrimo.Entities.board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Main
{
    public abstract class BoardState
    {
        public GameScreen screen;
        public Board board;
        public Bag bag;

        public void StartState(GameScreen screen, Board board)
        {
            this.screen = screen;
            this.board = board;
            bag = screen.Bag;

            screen.state = this;
            OnStateStart();
        }

        protected abstract void OnStateStart();
        public abstract void TickState();
    }
}
