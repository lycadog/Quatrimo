using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Main
{
    public class TickBoardState : BoardState
    {

        //tick every block on the board and process the board again if it is updated
        //next state: FinalizeTurnState
        public override void TickState()
        {
            throw new NotImplementedException("TickBoardState's TickState method was somehow ran.");
        }

        protected override void OnStateStart()
        {
            TickBlocks();

            if (screen.boardUpdated)
            {
                screen.StartState(new ProcessScoringState());
                return;
            }

            screen.StartState(new FinalizeTurnState());
        }

        void TickBlocks()
        {
            foreach(var block in screen.placedBlocks)
            {
                if (block.ticked) { continue; }

                block.Tick();
            }
        }
    }
}
