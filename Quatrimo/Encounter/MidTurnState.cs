using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Quatrimo.Keybinds;

namespace Quatrimo.Main
{
    public class MidTurnState : BoardState
    {
        double piecefallTimer = -.6; //set timers to negative to give more reaction time when a piece is first played
        double placeTimer = -.6;

        float leftMoveCooldown = 0;
        float rightMoveCooldown = 0;
        float fastfallCooldown = 0;
        

        //initialize the falling piece and make it fall to the bottom of the board
        //next state: ProcessScoringState
        public override void TickState()
        {
            

            if (piecefallTimer >= .6)
            {
                if (screen.CurrentPiece.Collides(0, -1)) //REPLACE WITH piece colliding later
                {
                    piecefallTimer = .1;
                    //if piece is colliding: check if it's time to place it
                    if (placeTimer >= 1)
                    {
                        PlacePieceAndEndState();
                        return;
                    }
                }
                else
                {
                    
                    //if piece is not colliding: move it and reset timers
                    screen.CurrentPiece.MoveByOffset(0, -1);
                    piecefallTimer = 0;
                    placeTimer = 0;
                    return;
                }
            }
            ProcessMovement();
            piecefallTimer += TimeManager.SecondDifference;
            placeTimer += TimeManager.SecondDifference;
        }

        protected override void OnStateStart()
        {
            
        }

        void PlacePieceAndEndState()
        {
            screen.CurrentPiece.Place();
            screen.Bag.DiscardCard(screen.Bag.currentCardIndex);
            screen.StartState(new ProcessScoringState());
        }

        void ProcessMovement()
        {

            if (Keybinds.Slam.Pushed)
            {
                screen.CurrentPiece.Slam();
                PlacePieceAndEndState();
                return;
            }

            //for movement keys, when key holds: do action once, wait until timeheld, then move rapidly
            if (Keybinds.Left.Pushed || (Keybinds.Left.TimeHeld > .14 && leftMoveCooldown > .03))
            {
                //TODO: check for collision later
                if(!screen.CurrentPiece.Collides(-1, 0))
                {
                    screen.CurrentPiece.MoveByOffset(-1, 0);
                    leftMoveCooldown = 0;
                }
                
            }
            else if (Keybinds.Right.Pushed || (Keybinds.Right.TimeHeld > .14 && leftMoveCooldown > .03))
            {
                //TODO: check for collision later
                if (!screen.CurrentPiece.Collides(1, 0))
                {
                    screen.CurrentPiece.MoveByOffset(1, 0);
                    rightMoveCooldown = 0;
                }
            }

            if (Keybinds.RotateLeft.Pushed)
            {
                screen.CurrentPiece.AttemptRotation(1);
            }else if (Keybinds.RotateRight.Pushed)
            {
                screen.CurrentPiece.AttemptRotation(-1);
            }

            if (Keybinds.Down.Pushed || Keybinds.Down.TimeHeld > .05 && fastfallCooldown > .01)
            {
                piecefallTimer += .600;
                fastfallCooldown = 0;
            }else if (Keybinds.Up.Held)
            {
                piecefallTimer = -.100;
            }


            leftMoveCooldown += TimeManager.SecondDifference;
            rightMoveCooldown += TimeManager.SecondDifference;
            fastfallCooldown += TimeManager.SecondDifference;

        }
    }
}
