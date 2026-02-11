using FlatRedBall;
using Quatrimo.Entities.block;
using Quatrimo.Factories;
using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quatrimo.Data;

namespace Quatrimo
{
    public class TestAttack : EnemyAttack
    {
        public TestAttack()
        {
            minCooldown = 2;
            maxCooldown = 8;
            turnsUntilAttack = 3;
        }

        public override void ExecuteAttack(GameScreen screen, Enemy enemy)
        {

            int repeats = FlatRedBallServices.Random.Next(0, 2);

            for (int i = 0; i <= repeats; i++)
            {
                int blockCount = FlatRedBallServices.Random.Next(1, 12);
                List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

                for (int i2 = 0; i2 < blockCount; i2++)
                {
                    list.Remove(FlatRedBallServices.Random.Next(0, list.Count - 1));
                }

                foreach (int x in list)
                {
                    Block block = BlockFactory.CreateNew();
                    block.CreateBlock(screen, 50, 30, new HsvColor(110, .7, .9));

                    for (int y = screen.trueBoardHeight - 1; true; y--)
                    {
                        if (block.CollidesFalling(x, y)) //Drop the block until it collides, then place it on the board
                        {
                            block.PlaceAt(x, y + 1);

                            break;
                        }
                    }

                }

            }
        }
    }
}
