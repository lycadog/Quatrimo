using Microsoft.Xna.Framework;
using Quatrimo.Data;
using Quatrimo.Entities;
using Quatrimo.Entities.board;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Main
{
    public class Bag
    {
        //thinking of how to implement bags and starter pieces for bags
        //like i want colors and textures for bags and everything
        List<Piece> pieces = [];
        ObjPool<Piece> pool;
        public HandUI MainHand;
        public List<Piece> Hand = [];

        public int currentCardIndex; //contains the corresponding index to the current selected piece/card, synced on play with screen

        int TextureLeft;
        int TextureRight;

        public Bag(PieceType[] pieceTypes, HsvColor[] colors, int textureLeft = 0, int textureTop = 0)
        {
            for(int i = 0; i < pieceTypes.Length; i++)
            {
                
                Piece piece = pieceTypes[i].GetPiece();


                if (textureTop != 0) //if texture value is not default: override texture
                {
                    piece.textureLeft = textureLeft;
                    piece.textureTop = textureTop;
                }

                if (colors.Length != 0) //if a color list is provided: override random colors
                {
                    piece.SetColor(colors[i]);
                }

                pieces.Add(piece);
            }
        }

        public void StartEncounter(HandUI hand)
        {
            MainHand = hand;
            CreateNewPool();
            DrawHand();
        }

        public void CreateNewPool()
        {
            pool = new ObjPool<Piece>([.. pieces], 6);
        }

        public void PlayCard(int index)
        {
            //do stuff here TODO
            currentCardIndex = index;
        }

        /// <summary>
        /// Draw cards based on hand size
        /// </summary>
        public void DrawHand()
        {
            for (int i = 0; i < PlayerStats.HandDrawSize; i++)
            {
                Piece piece = pool.getRandom(out var entry);
                AddPiece(piece);
                entry.subtractWeight(3);
            }
            MainHand.UpdateHand();
        }

        /// <summary>
        /// Add a piece to the hand and create a card for it
        /// </summary>
        /// <param name="piece"></param>
        public void AddPiece(Piece piece)
        {
            Hand.Add(piece);

            PieceCard card = Factories.PieceCardFactory.CreateNew();
            MainHand.AddCard(card);
            card.Initialize(piece);
        }

        /// <summary>
        /// Remove card from the hand
        /// </summary>
        /// <param name="index"></param>
        public void DiscardCard(int index)
        {
            MainHand.DiscardCard(index);
            Hand.RemoveAt(index);
        }

        public void SelectCard(int index)
        {
            MainHand.SelectCard(index);
        }

        public void DeselectCard()
        {
            MainHand.DeselectCard();
        }


        /// <summary>
        /// Tick all pieces, resetting their weight and progressing their piecetags
        /// </summary>
        public void Tick()
        {
            //todo: add piecetag ticking here
            foreach(var entry in pool.entries)
            {
                entry.addWeight(1);
                if(entry.weight > 6)
                {
                    entry.setWeight(6);
                }
            }

            if (Hand.Count == 0)
            {
                DrawHand();
            }
        }
        
        
        
    }
}
