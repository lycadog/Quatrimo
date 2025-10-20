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
        List<int> ScoredRows = [];

        public ProcessScoringState(int boardHeight)
        {
            RowUpdated = new bool[boardHeight];
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

        }

        protected override void OnStateStart()
        {
            for (int y = 0; y < screen.boardHeight; y++)
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
                    ScoredRows.Add(y);
                }


            }
        }
    }
}
