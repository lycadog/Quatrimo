using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Encounter
{
    public abstract class Enemy
    {
        public double maxHealth;
        public double health;

        public EnemyAttack activeAttack;

        EnemyAttack[] attackPool;

        public abstract void TakeDamage(double damage);

    }
}
