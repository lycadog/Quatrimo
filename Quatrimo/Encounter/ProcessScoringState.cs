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
        public bool[] RowUpdated;
        List<RowScorer> RowScorers = [];

        public ProcessScoringState(int boardHeight)
        {
            RowUpdated = new bool[boardHeight + 8];
            Array.Fill(RowUpdated, true);
        }
        public ProcessScoringState(bool[] updatedRows)
        {
            RowUpdated = updatedRows;
        }

        //process scoring and starting scoring animations, then waiting for them to finish before continuing
        // (with if statement not stupid interrupt system of before)
        //next state: TickBoardState
        public override void TickState()
        {
            bool complete = true;
            foreach(var scorer in RowScorers)
            {
                scorer.Iterate();
                if (!scorer.completed)
                {
                    complete = false;
                }
            }

            if (complete)
            {
                screen.StartState(new StartTurnAndWaitState());
            }
        }

        protected override void OnStateStart()
        {
            for (int y = 0; y < screen.boardHeight+8; y++)
            {
                if (!RowUpdated[y]) //if row not updated: skip row
                {
                    continue;
                }

                int nonScorableBlocks = 0;
                bool rowScorable = true; //default to true, if any block stops it from being scorable it will be set to false

                for (int x = 0; x < screen.boardWidth; x++)//iterate through row blocks
                {
                    if (!screen.blockboard[x, y].Scorable) //if a block isn't scorable check if we need to stop scoring
                    {
                        nonScorableBlocks++;
                        if(nonScorableBlocks > RunData.EmptySpacesAllowedForScoring)
                        {
                            rowScorable = false;
                            break;
                        }
                    }
                }
                //finish processing for this row

                if (rowScorable)
                {
                    RowScorers.Add(new RowScorer(screen, y));
                }
            }
        }
    }
}
