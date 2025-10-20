using Quatrimo.Data;
using Quatrimo.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public class StarterBag
    {
        PieceType[] pieces;
        HsvColor[] colors;

        int pieceTextureLeft;
        int pieceTextureTop;

        public string name;

        /// <summary>
        /// Create a bag using specified colors. Not specifying texture disables override
        /// </summary>
        /// <param name="pieces"></param>
        /// <param name="colors"></param>
        /// <param name="name"></param>
        /// <param name="pieceTextureLeft"></param>
        /// <param name="pieceTextureTop"></param>
        public StarterBag(PieceType[] pieces, HsvColor[] colors, string name, int pieceTextureLeft = 0, int pieceTextureTop = 0)
        {
            this.pieces = pieces;
            this.colors = colors;
            this.pieceTextureLeft = pieceTextureLeft;
            this.pieceTextureTop = pieceTextureTop;
            this.name = name;
        }

        /// <summary>
        /// Create a bag using random colors. Not specifying texture disables override
        /// </summary>
        /// <param name="pieces"></param>
        /// <param name="name"></param>
        /// <param name="pieceTextureLeft"></param>
        /// <param name="pieceTextureTop"></param>
        public StarterBag(PieceType[] pieces, string name, int pieceTextureLeft = 0, int pieceTextureTop = 0 )
        {
            this.pieces = pieces;
            this.colors = [];
            this.pieceTextureLeft = pieceTextureLeft;
            this.pieceTextureTop = pieceTextureTop;
            this.name = name;
        }

        //TODO: on game start turn all starter bags into global normal bags, so in bag selection you can easily see the piece previews
        //keep a seperate empty bag for the current selected bag

        public Bag CreateBag()
        {
            return new Bag(pieces, colors, pieceTextureLeft, pieceTextureTop);
        }

    }
}
