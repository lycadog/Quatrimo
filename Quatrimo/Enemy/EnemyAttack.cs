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

        public abstract void DoAttack(GameScreen screen, Enemy enemy);
    }
}
