using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Encounter
{
    public abstract class EnemyAttack
    {
        
        public int turnsUntilAttack;

        public abstract void DoAttack(GameScreen screen);
    }
}
