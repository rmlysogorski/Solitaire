using Microsoft.Xna.Framework;
using Solitare.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare.Models
{
    public class Layout
    {
        private static float cardScale = 0.30f;
        //private static int bottomRowTop = 730;
        private static int tableauTop = 50;
        private static int verticalOffset = 35;

        private static Vector2 menuPlayButton = new Vector2(1260, 935);
        private static Vector2 menuExitButton = new Vector2(1110, 935);
        private static int buttonRadius = 60;

        private static Vector2 playAgain = new Vector2(100, 100);
        private static Vector2 deck = new Vector2(50, 775);
        private static Vector2 drawPile = new Vector2(230, 775);
        private static Vector2 wastePile = new Vector2(410, 775);
        private static Vector2 heartsPile = new Vector2(1700, 100);
        private static Vector2 spadesPile = new Vector2(1700, 325);
        private static Vector2 diamondsPile = new Vector2(1700, 550);
        private static Vector2 clubsPile = new Vector2(1700, 775);
        private static Vector2 tableau1 = new Vector2(330, tableauTop);
        private static Vector2 tableau2 = new Vector2(510, tableauTop);
        private static Vector2 tableau3 = new Vector2(690, tableauTop);
        private static Vector2 tableau4 = new Vector2(870, tableauTop);
        private static Vector2 tableau5 = new Vector2(1050, tableauTop);
        private static Vector2 tableau6 = new Vector2(1230, tableauTop);
        private static Vector2 tableau7 = new Vector2(1410, tableauTop);

        public static float CardScale { get => cardScale; }
        public static int VerticalOffset { get => verticalOffset; }

        public static Vector2 MenuPlayButton { get => menuPlayButton; }
        public static Vector2 MenuExitButton { get => menuExitButton; }
        public static int ButtonRadius { get => buttonRadius; }

        public static Vector2 PlayAgain { get => playAgain; }
        public static Vector2 Deck { get => deck; }
        public static Vector2 DrawPile { get => drawPile; }
        public static Vector2 WastePile { get => wastePile; }
        public static Vector2 HeartsPile { get => heartsPile; }
        public static Vector2 SpadesPile { get => spadesPile; }
        public static Vector2 DiamondsPile { get => diamondsPile; }
        public static Vector2 ClubsPile { get => clubsPile; }
        public static Vector2 Tableau1 { get => tableau1; }
        public static Vector2 Tableau2 { get => tableau2; }
        public static Vector2 Tableau3 { get => tableau3; }
        public static Vector2 Tableau4 { get => tableau4; }
        public static Vector2 Tableau5 { get => tableau5; }
        public static Vector2 Tableau6 { get => tableau6; }
        public static Vector2 Tableau7 { get => tableau7; }

        public static Rectangle MakeCardBox(Vector2 cardPosition)
        {
            return new Rectangle(
                (int)cardPosition.X,
                (int)cardPosition.Y,
                (int)(GameState.cardBack.SpriteTexture.Width * cardScale),
                (int)(GameState.cardBack.SpriteTexture.Height * cardScale)
                );
        }
    }
}
