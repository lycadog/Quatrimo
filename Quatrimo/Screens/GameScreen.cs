using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using Microsoft.Xna.Framework;

using Quatrimo.Entities;
using Quatrimo.Entities.block;
using Quatrimo.Entities.board;
using Quatrimo.GumRuntimes;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo.Main;
using Quatrimo.Data;
using System.Reflection;
using System.Diagnostics;


namespace Quatrimo.Screens
{
    public partial class GameScreen
    {
        public BoardState state;

        public Block[,] blockboard;
        public bool[] RowUpdated; //if row of Y index has been updated this turn

        public Bag Bag;
        public Piece CurrentPiece;
        public int boardWidth = 12;
        public int trueBoardHeight = 28;
        public int visualBoardHeight = 20;

        //todo: maybe more possible values for updated board like updated rows

        //score variables
        public List<Block> scoredBlocks = [];
        public int level = 0;
        public int rowsRequiredForLevelup = 4;
        public int rowsCleared = 0;

        public double totalScore = 0;
        public double turnScore = 0;
        public double turnTimes = 1;

        public int ActiveAnimCount { get => ScoreAnimations.Count; }

        private void CustomInitialize()
        {
            CameraInstance.BackgroundColor = new Color(2, 0, 40);
            InitializeBoard(boardWidth, visualBoardHeight);

            RowUpdated = new bool[trueBoardHeight];

            Bag = GlobalData.magnetBag.CreateBag();
            Bag.StartEncounter(MainHand);

            StartState(new StartTurnAndWaitState());
        }

        private void CustomActivity(bool firstTimeCalled)
        {
            FlatRedBall.Debugging.Debugger.Write("ActiveAnimCount: " + ActiveAnimCount);
            Keybinds.UpdateBinds();
            state.TickState();
        }

        public void InitializeBoard(int width, int height)
        {
            blockboard = new Block[width, height + 8];
            MainBoard.GenerateGraphics(width, height);
            MainHand.SetPosition(width);

            for(int x = 0; x < width; x++) //Initialize the board with empty blocks
            {
                for(int y = 0; y < height + 8; y++)
                {
                    SetEmpty(x, y);
                }
            }
        }

        public void PlayCard(int index)
        {
            Bag.PlayCard(index);
            CurrentPiece = Bag.Hand[index];
            CurrentPiece.MoveToLayer(FalingBlocksLayer);
            CurrentPiece.AttachTo(MainBoard);
            CurrentPiece.Play(this);
        }

        public ScoreAnimation ScoreBlock(Block block, int index = 0)
        {
            return ScoreBlock(block, index, HsvColor.White);
        }

        public ScoreAnimation ScoreBlock(Block block, int index, HsvColor color)
        {
            ScoreAnimation anim = Factories.ScoreAnimationFactory.CreateNew();
            AttachToBoard(anim);
            anim.MoveToLayer(FalingBlocksLayer);
            anim.SetColor(color);
            anim.StartScoreAnimation(block.boardX, block.boardY, index);

            block.Score();
            scoredBlocks.Add(block);

            turnScore += block.score;
            turnTimes += block.times;
            return anim;
        }

        public void DiscardPlayedCard()
        {

        }

        public void StartState(BoardState state)
        {
            state.StartState(this, MainBoard);
        }
        
        /// <summary>
        /// Attach PositionedObject to the main board
        /// </summary>
        /// <param name="entity"></param>
        public void AttachToBoard(PositionedObject entity)
        {
            entity.AttachTo(MainBoard);
        }

        /// <summary>
        /// Set position on main board as empty
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetEmpty(int x, int y)
        {
            blockboard[x, y] = new EmptyBlock(this, x, y);
        }

        

        private void CustomDestroy()
        {
            SpriteManager.RemoveSpriteList(Sprites);
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {

        }



    }
}
