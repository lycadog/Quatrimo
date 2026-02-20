using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class TestSlime : Enemy
    {
        public TestSlime()
        {
            maxHealth = 300;
            health = 300;
            InitializeAttacks();
        }

        public override void InitializeAttacks()
        {
            attackPool = [new BlockPlacementAttack(){
                minBlocksDropped = 12,
                maxBlocksDropped = 12
            }


                ];
        }

        public override void LevelUp(int level)
        {
        }

        public override void TakeDamage(double damage)
        {
            
        }
    }
}
