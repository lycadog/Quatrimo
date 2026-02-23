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

        public bool attackOnCooldown = true;
        public int currentAttackCooldown = 1;

        protected int minAttackCooldown = 2;
        protected int maxAttackCooldown = 8;

        protected EnemyAttack[] attackPool;


        public virtual void Update(GameScreen screen)
        {
            if (attackOnCooldown) //if cooldown state
            {
                currentAttackCooldown -= 1;

                if(currentAttackCooldown <= 0)
                {
                    activeAttack = attackPool[FlatRedBallServices.Random.Next(attackPool.Length)];
                    activeAttack.PrepareAttack(screen, this); //prepare a random attack!
                    attackOnCooldown = false; //we are no longer on cooldown
                }
                return; //don't run non-cooldown code until next turn
            }

            activeAttack.turnsUntilAttack -= 1;



            if (activeAttack.turnsUntilAttack <= 0)
            {
                
                activeAttack.ExecuteAttack(screen, this);
                currentAttackCooldown = FlatRedBallServices.Random.Next(minAttackCooldown, maxAttackCooldown);
            }
        }

        public abstract void InitializeAttacks();
        public abstract void TakeDamage(double damage);
        public abstract void LevelUp(int level);
    }
}
