using Microsoft.Xna.Framework;
using Quatrimo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class BagBlock
    {
        public BlockData data;
        public int localX, localY;

        public HsvColor hsvColor;

        public BagBlock(int typeID, int localX, int localY, HsvColor color)
        {
            data = GlobalData.blocks[typeID];
            this.localX = localX;
            this.localY = localY;
            this.hsvColor = color;
            
        }
    }
}
