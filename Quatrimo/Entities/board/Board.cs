using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;

namespace Quatrimo.Entities.board
{
    public partial class Board
    {
        public int Width;
        public int Height;

        public void GenerateGraphics(int width, int height)
        {
            int x = width + 2, y = height + 2;
            x *= 10; y *= 10;

            int xOffset = x /2 -5, yOffset = y / 2;
            X = -xOffset; Y = -yOffset;

            BorderBG.RelativeX = xOffset; BorderBG.RelativeY = yOffset - 5;
            BorderBG.Width = x; BorderBG.Height = y;
            //FlatRedBall.Debugging.Debugger.CommandLineWrite("X/Y10: " + X + ", " + Y);

            

            BorderD.Width = x - 20; BorderD.RelativeX = (x - 20) / 2 + 5;
            BorderU.Width = x - 20; BorderU.RelativeX = (x - 20) / 2 + 5; BorderU.RelativeY = y - 10;

            BorderL.Height = y - 20; BorderL.RelativeY = (y - 20) / 2 + 5;
            BorderR.Height = y - 20; BorderR.RelativeX = x - 10; BorderR.RelativeY = (y - 20) / 2 + 5;

            BorderDR.RelativeX = x - 10;
            BorderUR.RelativeX = x - 10; BorderUR.RelativeY = y - 10;
            BorderUL.RelativeY = y - 10;

            GenerateBGGraphics(width, height);
        }

        void GenerateBGGraphics(int width, int height)
        {
            Color[] colors = [new Color(10, 10, 10), new Color(15, 15, 15)];
            int colorCounter = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    Sprite sprite = SpriteManager.AddSprite(atlas);
                    SpriteManager.AddToLayer(sprite, LayerProvidedByContainer);
                    sprite.RelativeX = x * 10 + 10; sprite.RelativeY = y * 10 + 10;
                    sprite.AttachTo(this);
                    BGSprites.Add(sprite);

                    sprite.Width = 10; sprite.Height = 10;

                    sprite.TopTexturePixel = 30;
                    sprite.BottomTexturePixel = 40;
                    sprite.RightTexturePixel = 10;

                    int colorFactor = Math.Abs(y - height / 2);

                    //todo: maybe scale up colorfactor on smaller boards to match the default brightening so the
                    //background isnt too dark
                    float colorMultiplier = (float)Math.Pow(colorFactor, 2) * 0.026f + 1;

                    Color color = colors[colorCounter % 2];
                    sprite.Color = new Color((byte)(color.R * colorMultiplier), (byte)(color.G * colorMultiplier), (byte)(color.B * colorMultiplier));
                    sprite.ColorOperation = FlatRedBall.Graphics.ColorOperation.ColorTextureAlpha;

                    colorCounter++;
                }
                colorCounter++;
            }
        }

        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {

        }

        private void CustomActivity()
        {
            
        }

        private void CustomDestroy()
        {
            foreach(var sprite in BGSprites)
            {
                SpriteManager.RemoveSprite(sprite);
            }

            /*foreach(var block in BlockList) may be necessary to uncomment. not entirely sure if the screen cleans up after code created objects or not
            {
                block.Destroy();
            }*/
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
