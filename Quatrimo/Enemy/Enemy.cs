using FlatRedBall;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public int currentAttackCooldown = 2;

        protected int minAttackCooldown = 2;
        protected int maxAttackCooldown = 8;

        protected EnemyAttack[] attackPool;

        public EnemyState state = EnemyState.Idle;

        public enum EnemyState
        {
            Idle,
            ChargingAttack,
            Attacking
        }

        /// <summary>
        /// Update enemy's attacks and behavior. Returns if FinalizeTurnState should end or wait for the attack to resolve. True = End
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual bool Update(GameScreen screen)
        {
            switch (state)
            {
                case EnemyState.Idle:

                    currentAttackCooldown -= 1;
                    if(currentAttackCooldown <= 0)
                    {
                        //prepare a random attack!
                        activeAttack = attackPool[FlatRedBallServices.Random.Next(attackPool.Length)];
                        activeAttack.PrepareAttack(screen, this);
                        
                        

                        //we are charging our attack so let's change state!
                        state = EnemyState.ChargingAttack;
                    }
                    return true;
                
                case EnemyState.ChargingAttack:

                    activeAttack.turnsUntilAttack -= 1;

                    if (activeAttack.turnsUntilAttack <= 0)
                    {
                        //Run the attack and set our state to attacking! Lets get 'em!!!!!
                        activeAttack.StartAttack(screen, this);
                        state = EnemyState.Attacking;

                        return false;
                        //We just started our attack so we need to wait to resolve it, so we return false
                    }

                    return true;
                    
                case EnemyState.Attacking:

                    if(activeAttack.ResolveAttack(screen, this))
                    {
                        currentAttackCooldown = FlatRedBallServices.Random.Next(minAttackCooldown, maxAttackCooldown);
                        state = EnemyState.Idle;

                        return true;
                    }

                    return false;
                //idk!!!!!!
                //check if attack is done or not
                //let attack handle if its resolved
                //if its true we need to do some stuff to change our state back to idle

                default:
                    throw new ArgumentOutOfRangeException("Enemy update ran with invalid EnemyState " + state);       
            }

        }

        public void HideTelegraphs()
        {
            if (state == EnemyState.Idle) { return; }
            activeAttack.HideTelegraphSprites();
        }

        public void UnhideTelegraphs()
        {
            if (state == EnemyState.Idle) { return; }
            activeAttack.UnhideTelegraphSprites();
        }

        public abstract void InitializeAttacks();
        public abstract void TakeDamage(double damage);
        public abstract void LevelUp(int level);
    }
}
