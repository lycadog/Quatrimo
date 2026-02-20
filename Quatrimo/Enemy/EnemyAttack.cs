using FlatRedBall;
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
        public int turnsUntilAttack = 0;
        protected int minCooldown = 2;
        protected int maxCooldown = 12;
        protected bool scalesWithLevel;

        public virtual void PrepareAttack(GameScreen screen, Enemy attacker)
        {
            turnsUntilAttack = FlatRedBallServices.Random.Next(minCooldown, maxCooldown);
        }

        public abstract void ExecuteAttack(GameScreen screen, Enemy enemy);
    }
}
