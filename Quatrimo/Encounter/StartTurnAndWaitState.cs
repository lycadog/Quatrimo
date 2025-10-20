using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using Quatrimo.Entities.board;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Main
{
    public class StartTurnAndWaitState : BoardState
    {
        int SelectedCard = -1;

        //todo: add piece position preview and moving where piece drops from
        //draw cards and initialize ui and other factors and then wait for player to select a card
        //next state MidTurnState
        public override void TickState()
        {
            if (InputManager.Keyboard.KeyPushed(Keys.Space))
            {
                //play selected piece if SelectedCard != -1
                if (SelectedCard != -1)
                {
                    
                    screen.PlayCard(SelectedCard);
                    screen.StartState(new MidTurnState());
                    return;
                }
            }

            if (InputManager.Keyboard.KeyPushed(Keys.D1))
            {
                SelectPiece(0);
            }else if (InputManager.Keyboard.KeyPushed(Keys.D2))
            {
                SelectPiece(1);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D3))
            {
                SelectPiece(2);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D4))
            {
                SelectPiece(3);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D5))
            {
                SelectPiece(4);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D6))
            {
                SelectPiece(5);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D7))
            {
                SelectPiece(6);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D8))
            {
                SelectPiece(7);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D9))
            {
                SelectPiece(8);
            }
            else if (InputManager.Keyboard.KeyPushed(Keys.D0))
            {
                SelectPiece(9);
            }
        }

        protected override void OnStateStart()
        {
            bag.Tick();
        }

        void SelectPiece(int index)
        {
            if (index >= bag.Hand.Count) { return; }
            if(SelectedCard == index)
            {
                bag.DeselectCard();
                SelectedCard = -1;
                return;
            }

            SelectedCard = index;
            bag.SelectCard(index);
        }
    }
}
