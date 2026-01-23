using FlatRedBall;
using Quatrimo.Screens;
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
        public int level;

        public EnemyAttack activeAttack;

        bool attackOnCooldown = true;
        int currentAttackCooldown;

        int minAttackCooldown;
        int maxAttackCooldown;

        EnemyAttack[] attackPool;

        protected Enemy(double maxHealth, double health, int level)
        {
            this.maxHealth = maxHealth;
            this.health = health;
            this.level = level;
        }

        public void Update(GameScreen screen)
        {
            if (attackOnCooldown)
            {
                currentAttackCooldown = -1;

                if(currentAttackCooldown == 0)
                {
                    activeAttack = attackPool[FlatRedBallServices.Random.Next(attackPool.Length)];
                    attackOnCooldown = false;
                }
                return;
            }

            activeAttack.turnsUntilAttack = -1;

            if(activeAttack.turnsUntilAttack == 0)
            {
                activeAttack.DoAttack(screen);
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
