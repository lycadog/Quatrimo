using FlatRedBall;
using Quatrimo.Entities.block;
using Quatrimo.Factories;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class TestAttack : EnemyAttack
    {
        public TestAttack()
        {
            turnsUntilAttack = 3;
        }

        public override void DoAttack(GameScreen screen, Enemy enemy)
        {
           
            int blockCount = FlatRedBallServices.Random.Next(1, 12);
            List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

            for(int i = 0; i < blockCount; i++)
            {
                list.Remove(FlatRedBallServices.Random.Next(0, list.Count - 1));
            }
            foreach (int x in list)
            {

                for (int y = screen.trueBoardHeight - 1; y >= 0; y--)
                {
                    if (screen.blockboard[x,y] is not EmptyBlock)
                    {
                        Block block = BlockFactory.CreateNew();
                        block.CreateBlock(screen, null, 0, 0, 50, 30);

                        block.MoveTo(x, y + 1);
                        block.UnhideSprites();
                        block.Place();
                        break;
                    }
                }
            }
        }
    }
}
