using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Main
{
    public class ProcessScoringState : BoardState
    {

        //process scoring and starting scoring animations, then waiting for them to finish before continuing
        // (with if statement not stupid interrupt system of before)
        //next state: TickBoardState
        public override void TickState()
        {
            bool complete = true;
            foreach(var scorer in screen.activeScorers)
            {
                scorer.Update();
                if (!scorer.completed)
                {
                    complete = false;
                }
            }

            if (complete && screen.ActiveAnimCount == 0)
            {
                screen.boardUpdated = false;
                endState();
            }
        }

        protected override void OnStateStart()
        {
            CheckUpdatedRows();
        }

        void endState()
        {

            //Remove scorers since they have already completed
            screen.activeScorers.Clear();

            //remove scored blocks. this will update their respective rows.
            foreach (var block in screen.scoredBlocks)
            {
                block.RemoveAndLower();
            }
            screen.scoredBlocks.Clear();

            //recheck said rows and return to repeat the state and score everything
            CheckUpdatedRows();

            if(screen.activeScorers.Count > 0)
            {
                return;
            }

            screen.StartState(new TickBoardState());
        }

        void CheckUpdatedRows()
        {
            //iterate through each row and check them all
            for (int y = 0; y < screen.trueBoardHeight; y++)
            {
                if (!screen.RowUpdated[y]) //if row not updated: skip row
                {
                    continue;
                }
                //Row is being processed, so uncheck as updated
                screen.RowUpdated[y] = false;

                int nonScorableBlocks = 0;
                bool rowScorable = true; //default to true, if any block stops it from being scorable it will be set to false

                for (int x = 0; x < screen.boardWidth; x++)//iterate through row blocks
                {
                    if (!screen.blockboard[x, y].Scorable) //if a block isn't scorable check if we need to stop scoring
                    {
                        nonScorableBlocks++;
                        if (nonScorableBlocks > PlayerStats.EmptySpacesAllowedForScoring)
                        {
                            rowScorable = false;
                            break;
                        }
                    }
                }
                
                //finish processing for this row
                if (rowScorable)
                {
                    screen.activeScorers.Add(new RowScorer(screen, y));
                }
            }
        }
    }
}
