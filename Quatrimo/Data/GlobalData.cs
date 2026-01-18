using FlatRedBall.Math;
using Microsoft.Xna.Framework;
using Quatrimo.Entities.block;
using Quatrimo.Entities.board;
using Quatrimo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quatrimo.BagTypes.Bagblocks;

namespace Quatrimo.Data
{
    public static class GlobalData
    {
        public static Func<Piece>[] pieces = [
            Piece.CreateNew
            ];

        public static BlockData[] blocks = [
            new BlockData(),
            new CursedBlockData(),
            new BombBlockData()
            ];

        

        public static PieceShape sLine = new(new int[,] { { 1 }, { 2 }, { 3 }, { 4 } }, 2, 0, 4, "Line");
        public static PieceShape sStick = new(new int[,] { { 1 }, { 2 }, { 3 } }, 1, 0, 3, "Stick");
        public static PieceShape sTwig = new(new int[,] { { 1 }, { 2 } }, 0, 0, 2, "Twig", -1);
        public static PieceShape sNub = new(new int[,] { { 1 } }, 0, 0, 1, "Nub");
        public static PieceShape sWedge = new(new int[,] { { 1, 0 }, { 2, 3 } }, 1, 0, 3, "Wedge", 0, -1);
        public static PieceShape sSlash = new(new int[,] { { 1, 0 }, { 0, 2 } }, 0, 0, 2, "Slash", -1, -1);

        public static PieceShape sSmallTee = new(new int[,] { { 0, 1 }, { 2, 2 }, { 0, 3 } }, 1, 1, 4, "Small Tee");
        public static PieceShape sSquare = new(new int[,] { { 1, 2 }, { 3, 4 } }, 1, 1, 4, "Square");
        public static PieceShape sLZPiece = new(new int[,] { { 0, 1, 2 }, { 3, 4, 0 } }, 1, 1, 4, "Left Zee");
        public static PieceShape sRZPiece = new(new int[,] { { 1, 2, 0 }, { 0, 3, 4 } }, 1, 1, 4, "Right Zee");
        public static PieceShape sLLPiece = new(new int[,] { { 1, 2, 3 }, { 4, 0, 0 } }, 1, 1, 4, "Left Elle");
        public static PieceShape sRLPiece = new(new int[,] { { 1, 0, 0 }, { 2, 3, 4 } }, 1, 1, 4, "Right Elle");

        public static PieceShape sBigTee = new(new int[,] { { 1, 2, 1 }, { 0, 3, 0 }, { 0, 4, 0 } }, 1, 1, 5, "Big Tee");
        public static PieceShape sLHatchet = new(new int[,] { { 1, 2 }, { 1, 2 }, { 3, 0 }, { 3, 0 } }, 2, 0, 6, "Left Hatchet", 0, -1);
        public static PieceShape sRHatchet = new(new int[,] { { 1, 0 }, { 1, 0 }, { 2, 3 }, { 2, 3 } }, 1, 0, 6, "Right Hatchet", -1, -1);
        public static PieceShape sDipole = new(new int[,] { { 1, 2 }, { 0, 0 }, { 3, 4 } }, 1, 1, 4, "Dipole");
        public static PieceShape sLHook = new(new int[,] { { 0, 1 }, { 2, 0 }, { 3, 0 } }, 1, 0, 3, "Left Hook", 0, -1);
        public static PieceShape sRHook = new(new int[,] { { 1, 0 }, { 2, 0 }, { 0, 3 } }, 1, 0, 3, "Right Hook", 0, -1);

        public static PieceShape sCorner = new(new int[,] { { 1, 1, 2 }, { 3, 0, 0 }, { 4, 0, 0 } }, 0, 2, 5, "Corner", -1, 1);
        public static PieceShape sRectangle = new(new int[,] { { 1, 1 }, { 2, 2 }, { 3, 3 } }, 1, 1, 6, "Rectangle");
        public static PieceShape sLPick = new(new int[,] { { 1, 0 }, { 2, 3 }, { 4, 0 }, { 4, 0 } }, 1, 1, 5, "Left Pick", -1);
        public static PieceShape sRPick = new(new int[,] { { 1, 0 }, { 1, 0 }, { 2, 3 }, { 4, 0 } }, 2, 1, 5, "Right Pick", -1);

        public static PieceShape sCaret = new(new int[,] { { 0, 1 }, { 2, 0 }, { 0, 3 } }, 1, 1, 3, "Caret");
        public static PieceShape sBoson = new(new int[,] { { 0, 1, 0 }, { 0, 2, 3 }, { 4, 0, 0 } }, 1, 1, 4, "Boson", 0, -1);
        public static PieceShape sLLepton = new(new int[,] { { 1, 2 }, { 0, 0 }, { 3, 0 } }, 1, 0, 3, "Left Lepton", 0, -1);
        public static PieceShape sRLepton = new(new int[,] { { 3, 0 }, { 0, 0}, { 1, 2} }, 1, 0, 3, "Right Lepton", 0, -1);
        public static PieceShape sStump = new(new int[,] { { 1, 0 }, { 2, 3 }, { 2, 3 }, { 4, 0 } }, 1, 1, 6, "Stump", -1 );

        //public static PieceShape s = new(new int[,] { { } })

        public static SimplePieceType sCursedNub = new(sNub, 0, 1);
        public static SimplePieceType sBombTwig = new(sTwig, 0, 2);

        public static StarterBag magnetBag = new([sBigTee.B, sLHatchet.B, sRHatchet.B, sDipole.B, sLHook.B, sRHook.B, sLine.B, sTwig.B, sWedge.B],
            [new HsvColor(334, .62, .97), new HsvColor(294, .77, .973), new HsvColor(280, .94, .94), new HsvColor(258, .65, .98), new HsvColor(163, .56, .97), new HsvColor(124, .66, .96), new HsvColor(348, .75, .9),new HsvColor(36, .73, 1), new HsvColor(240, .73, .95)],
                "magnet bag", 80, 30);

        public static StarterBag quantumBag = new([sCaret.B, sBoson.B, sLLepton.B, sRLepton.B, sStump.B, sSlash.B, sSmallTee.B, sLine.B, sTwig.B, sNub.B], [
            new HsvColor(330, 1, .93), new HsvColor(310, .9, .91), new HsvColor(288, .83, .97), new HsvColor(274, .88, 1), new HsvColor(266, .83, .85), new HsvColor(258, .91, 1), new HsvColor(180, .88, 1), new HsvColor(160, .92, .92), new HsvColor(60, .88, 1), new HsvColor(50, .75, 1)
            ], "quantum bag", 0, 40);

        public static StarterBag debugBag = new([sCursedNub, sLHatchet.B, sTwig.B, sBombTwig], "debug");


    }
}
