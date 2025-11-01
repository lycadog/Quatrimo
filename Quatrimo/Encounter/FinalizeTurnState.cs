using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Main
{
    public class FinalizeTurnState : BoardState
    {

        //tally up the final score to be added and execute enemy's turn
        //next state: StartTurnAndWaitState
        public override void TickState()
        {
            throw new NotImplementedException();
        }

        protected override void OnStateStart()
        {
            foreach(var block in screen.placedBlocks)
            {
                block.justPlaced = false;
                block.scored = false;
                block.ticked = false;

            }
            screen.Bag.DeselectCard();

            screen.StartState(new StartTurnAndWaitState());
        }
        
    }
}
