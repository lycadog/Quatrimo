using FlatRedBall;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public abstract class EnemyAttack
    {
        public int turnsUntilAttack = 4;
        public int minCooldown = 6;
        public int maxCooldown = 14;
        public bool scalesWithLevel;

        public static readonly Texture2D atlas = FlatRedBallServices.Load<Texture2D>("Content/atlas.png");

        public virtual void PrepareAttack(GameScreen screen, Enemy attacker)
        {
            turnsUntilAttack = FlatRedBallServices.Random.Next(minCooldown, maxCooldown);
        }

        /// <summary>
        /// Begin the attack process, leading into UpdateAttack
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="enemy"></param>
        public abstract void StartAttack(GameScreen screen, Enemy enemy);

        /// <summary>
        /// Update attack visuals and animations. Returns true when all animations are complete
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public abstract bool ResolveAttack(GameScreen screen, Enemy enemy);

        /// <summary>
        /// Hides attack visuals while the player is actively playing
        /// </summary>
        public abstract void HideTelegraphSprites();

        /// <summary>
        /// Unhides sprites after player is done
        /// </summary>
        public abstract void UnhideTelegraphSprites();

    }
}
