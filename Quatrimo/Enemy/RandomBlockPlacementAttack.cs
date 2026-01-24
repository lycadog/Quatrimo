using Quatrimo.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public abstract class RandomBlockPlacementAttack : EnemyAttack
    {
        //Range of blocks dropped
        int totalMin;
        int totalMax;
        //Range of allowed depth, if total block count exceeds the block count of the depth rolled, larger depth (within max) will happen
        int depthMin;
        int depthMax;

        //Chance of all the blocks being placed next to eachother as opposed to randomly.
        float groupingCoefficient = 0.5f;
        

        int[] blockTypes;


 
    }
}
