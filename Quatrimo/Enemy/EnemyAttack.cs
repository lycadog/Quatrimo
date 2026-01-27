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
        public int turnsUntilAttack;
        protected int minCooldown;
        protected int maxCooldown;
        protected bool scalesWithLevel;

        public virtual void PrepareAttack(Enemy attacker)
        {
            turnsUntilAttack = FlatRedBallServices.Random.Next(minCooldown, maxCooldown);
        }

        public abstract void ExecuteAttack(GameScreen screen, Enemy enemy);
    }
}
