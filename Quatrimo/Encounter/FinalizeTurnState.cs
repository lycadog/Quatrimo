using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            screen.Enemy.Update(screen); //need to check board again after an attack

            //wait until the attack animations are fully done TODO

            if (screen.boardUpdated)
            {
                screen.StartState(new ProcessScoringState());
                return;
            }



            foreach (var block in screen.placedBlocks)
            {
                block.justPlaced = false;
                block.scored = false;
                block.ticked = false;

            }
            screen.Bag.DeselectCard();

            screen.RowsCleared += screen.turnRowsCleared;

            while(screen.RowsCleared >= screen.rowsRequiredForLevelup) //increment level!
            {
                Debug.WriteLine(screen.RowsCleared);
                screen.level += 1;
                screen.levelTimes += 0.5f;

                screen.RowsCleared -= screen.rowsRequiredForLevelup;
                screen.rowsRequiredForLevelup += 2;
            }

            if(screen.turnRowsCleared >= 4)
            {
                screen.turnScore *= screen.levelTimes;
            }

            screen.turnRowsCleared = 0;

            screen.Enemy.health -= screen.turnScore;
            screen.turnScore = 0;

            //todo: update UI stuff too
            screen.UpdateUI();
            screen.StartState(new StartTurnAndWaitState());
        }
        
    }
}
