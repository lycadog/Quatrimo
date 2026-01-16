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
using Quatrimo.Encounter;
using FlatRedBall.Graphics;


namespace Quatrimo.Screens
{
    public partial class GameScreen
    {
        public BoardState state;

        public List<Block> placedBlocks = [];
        public List<Block> scoredBlocks = [];
        public List<Scorer> activeScorers = [];

        public Block[,] blockboard;
        public bool[] RowUpdated; //if row of Y index has been updated this turn
        public bool boardUpdated = false;

        public Bag Bag;
        public Piece CurrentPiece;
        public int boardWidth = 12;
        public int trueBoardHeight;
        public int visualBoardHeight = 20;

        public Enemy Enemy;

        public int level = 0;
        public int rowsRequiredForLevelup = 4;
        public int rowsCleared = 0;
        public double totalScore = 0;
        public double turnScore = 0;
        public double turnTimes = 1;

        public int ActiveAnimCount { get => ScoreAnimations.Count; }

        public RollingNumberBar ScoreBar;

        private void CustomInitialize()
        {
            FlatRedBallServices.GraphicsDeviceManager.HardwareModeSwitch = false;
            CameraInstance.BackgroundColor = new Color(2, 0, 40);
            trueBoardHeight = visualBoardHeight + 8;
            RowUpdated = new bool[trueBoardHeight];
            InitializeBoard();

            GumScreen.ButtonStandardInstance.Click += (IWindow window) => { FlatRedBallServices.Game.Exit(); };

            Bag = GlobalData.debugBag.CreateBag();
            Bag.StartEncounter(MainHand);

            StartState(new StartTurnAndWaitState());
        }

        private void CustomActivity(bool firstTimeCalled)
        {
            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.F4))
            {
                if (FlatRedBallServices.GraphicsOptions.IsFullScreen)
                {
                    FlatRedBallServices.GraphicsOptions.IsFullScreen = false;
                }
                else
                {
                    var displayMode = FlatRedBallServices.GraphicsDevice.DisplayMode;
                    FlatRedBallServices.GraphicsOptions.SetFullScreen(displayMode.Width, displayMode.Height);
                }
            }

            Keybinds.UpdateBinds();
            state.TickState();
        }

        public void InitializeBoard()
        {
            blockboard = new Block[boardWidth, trueBoardHeight + 8];
            MainBoard.GenerateGraphics(boardWidth, visualBoardHeight);
            MainHand.SetPosition(boardWidth);
            for(int x = 0; x < boardWidth; x++) //Initialize the board with empty blocks
            {
                for(int y = 0; y < trueBoardHeight; y++)
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
            ScoreAnimation anim = Factories.ScoreAnimationFactory.CreateNew(FalingBlocksLayer);
            AttachToBoard(anim);
            anim.SetColor(color);
            anim.StartScoreAnimation(block.boardX, block.boardY, index);

            block.Score();
            scoredBlocks.Add(block);

            turnScore += block.score;
            turnTimes += block.times;
            boardUpdated = true;
            ScoreNumberBar.UpdateBoxes((int)turnScore);
            return anim;
        }

        public void DamageEnemy(double damage)
        {
            Enemy.TakeDamage(damage);
            EnemyHPNumberBar.UpdateBoxes((int)Enemy.health);

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
            foreach(var block in blockboard)
            {
                block?.Destroy();
            }
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {

        }



    }
}
