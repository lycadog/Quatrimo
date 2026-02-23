using FlatRedBall;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.Localization;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo.Data;
using Quatrimo.Encounter;
using Quatrimo.Entities;
using Quatrimo.Entities.block;
using Quatrimo.Entities.board;
using Quatrimo.GumRuntimes;
using Quatrimo.Main;
using System;
using System.Collections.Generic;



namespace Quatrimo.Screens
{
    public partial class GameScreen
    {
        public BoardState state;

        public List<Block> placedBlocks = [];
        public List<Block> scoredBlocks = [];
        public List<Scorer> activeScorers = [];
        public List<Scorer> queuedScorers = [];

        Block[,] blockboard;

        public Block this[int x, int y]
            {
            get => blockboard[x, y];
            set {
                blockboard[x, y] = value;
                RowUpdated[y] = true;
                boardUpdated = true; }
            }
        public bool[] RowUpdated; //if row of Y index has been updated this turn
        public bool boardUpdated = false;

        public Bag Bag;
        public Piece CurrentPiece;
        public int boardWidth = 12;
        public int trueBoardHeight;
        public int visualBoardHeight = 20;

        public Enemy Enemy;

        public int level = 0;
        public float levelTimes = 1;
        public int rowsRequiredForLevelup = 4;
        public int turnRowsCleared = 0;
        public int RowsCleared = 0;
        public double turnScore = 0;

        public int ActiveAnimCount { get => ScoreAnimations.Count; }

        public RollingNumberBar ScoreBar;

        private void CustomInitialize()
        {
            FlatRedBallServices.GraphicsDeviceManager.HardwareModeSwitch = false;
            CameraInstance.BackgroundColor = new Color(2, 0, 40);
            trueBoardHeight = visualBoardHeight + 8;
            RowUpdated = new bool[trueBoardHeight];
            Enemy = new TestSlime();
            InitializeBoard();

            //GumScreen.ButtonStandardInstance.Click += (IWindow window) => { FlatRedBallServices.Game.Exit(); };

            Bag = GlobalData.quantumBag.CreateBag();
            Bag.StartEncounter(MainHand);

            UpdateUI();

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
            blockboard = new Block[boardWidth, trueBoardHeight];
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

        public void QueueScorer(Scorer scorer) //this is bad and is used very bad.
        {
            queuedScorers.Add(scorer);
        }

        public TemporaryAnimation ScoreBlock(Block block, int index = 0)
        {
            return ScoreBlock(block, index, HsvColor.White);
        }

        public TemporaryAnimation ScoreBlock(Block block, int index, HsvColor color)
        {
            TemporaryAnimation anim = Factories.TemporaryAnimationFactory.CreateNew(FalingBlocksLayer);
            AttachToBoard(anim);
            anim.SetColor(color);
            anim.StartScoreAnimation(block.boardX, block.boardY, index);

            block.Score();
            scoredBlocks.Add(block);

            turnScore += block.score;
            //turnTimes += block.times;
            boardUpdated = true;
            ScoreNumberBar.UpdateBoxes((int)turnScore);
            return anim;
        }

        public bool IsOutsideBounds(int x, int y)
        {
            return x >= boardWidth || x < 0 || y >= trueBoardHeight || y < 0;
        }

        public void DamageEnemy(double damage)
        {
            Enemy.TakeDamage(damage);
            EnemyHPNumberBar.UpdateBoxes((int)Enemy.health);
        }

        public void UpdateUI()
        {
            string number = "???";
            if (Enemy.attackOnCooldown == false)
            {
                number = Enemy.activeAttack.turnsUntilAttack.ToString();
            }

            GumScreen.timeUntilAttack.Text = $"ATTACK IN \n{number} turns";

            GumScreen.levelTimes.Text = $"| X: {levelTimes}";
            GumScreen.playerLevel.Text = $"LVL: {level}";
            GumScreen.enemyHP.Text = $"HP: {Enemy.health}/{Enemy.maxHealth}";
            ScoreNumberBar.ClearBox();

            //todo: check attack current state after it's updated and update ui accordingly
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

        public void AttachBlockToBoard(Block block, bool placed = true)
        {
            block.AttachTo(MainBoard);
            block.MoveToLayer(PlacedBlocksLayer);

            if (!placed)
            {
                return;
            }
            TemporaryAnimation anim = Factories.TemporaryAnimationFactory.CreateNew(FalingBlocksLayer);
            AttachToBoard(anim);
            anim.StartScoreAnimation(block.boardX, block.boardY, 2);

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
