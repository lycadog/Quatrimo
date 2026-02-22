using Quatrimo.Data;
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
            maxHealth = 1000;
            health = 1000;
            InitializeAttacks();
        }

        public override void InitializeAttacks()
        {
            attackPool = [new BlockPlacementAttack(){
                color = new HsvColor(137, .8, .9),
                useRandomColor = false
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
