using FlatRedBall;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public abstract class Enemy
    {
        public double maxHealth;
        public double health;
        public int level = 0;

        public EnemyAttack activeAttack;

        protected bool attackOnCooldown = true;
        public int currentAttackCooldown = 10;

        protected int minAttackCooldown;
        protected int maxAttackCooldown;

        protected EnemyAttack[] attackPool;


        public virtual void Update(GameScreen screen)
        {
            if (attackOnCooldown)
            {
                currentAttackCooldown -= 1;

                if(currentAttackCooldown == 0)
                {
                    activeAttack = attackPool[FlatRedBallServices.Random.Next(attackPool.Length)];
                    attackOnCooldown = false;
                }
                return;
            }

            activeAttack.turnsUntilAttack -= 1;

            if(activeAttack.turnsUntilAttack == 0)
            {
                activeAttack.DoAttack(screen, this);
                currentAttackCooldown = FlatRedBallServices.Random.Next(minAttackCooldown, maxAttackCooldown);

                activeAttack = null;
                attackOnCooldown = true;
            }
            
            
        }

        public abstract void InitializeAttacks();
        public abstract void TakeDamage(double damage);
        public abstract void LevelUp(int level);
    }
}
