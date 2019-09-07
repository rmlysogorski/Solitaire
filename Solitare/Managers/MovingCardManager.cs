using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Solitare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare.Managers
{
    public class MovingCardManager
    {
        private Card movingCard;
        public Card MovingCard { get => movingCard; set => movingCard = value; }

        public MovingCardManager()
        {
            movingCard = new Card();
        }
        public bool CheckForMovement(DeckManager deckManager, MouseState mState, MouseState pmState, Vector2 smPosition)
        {
            if (movingCard.IsMoving)
            {
                return true;
            }
            else
            {
                foreach(var keyValuePair in deckManager.CardsInPlay)
                {
                    if(keyValuePair.Key != "Hearts" 
                        && keyValuePair.Key != "Spades" 
                        && keyValuePair.Key != "Diamonds"
                        && keyValuePair.Key != "Clubs"
                        && keyValuePair.Value.Length > 0
                        && keyValuePair.Value[keyValuePair.Value.Length- 1].CardBox.Contains(smPosition)
                        && !keyValuePair.Value[keyValuePair.Value.Length - 1].IsFaceDown
                        && MouseInput.CheckForSingleClick(pmState))
                    {                        
                        movingCard = keyValuePair.Value[keyValuePair.Value.Length - 1];
                        movingCard.IsMoving = true;
                        deckManager.CardsInPlay[keyValuePair.Key] = deckManager.DownsizePile(deckManager.CardsInPlay[keyValuePair.Key]);
                        if(keyValuePair.Key == "Deck")
                        {
                            deckManager.CardWasAlreadyDrawn = false;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckIfMovementStopped(MouseState mState)
        {
            return (mState.LeftButton == ButtonState.Released && movingCard.IsMoving == true);
        }

        public void ReturnCardToPile(DeckManager deckManager)
        {
            if(movingCard.PilePosition == Layout.DrawPile)
            {
                deckManager.AddCardToPile(movingCard, "Deck");
            }
            else if(movingCard.PilePosition == Layout.WastePile)
            {
                deckManager.AddCardToPile(movingCard, "Waste");
            }
            else if(movingCard.PilePosition == new Vector2(Layout.Tableau1.X, Layout.Tableau1.Y + deckManager.CardsInPlay["Tableau1"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau1");
            }
            else if (movingCard.PilePosition == new Vector2(Layout.Tableau2.X, Layout.Tableau2.Y + deckManager.CardsInPlay["Tableau2"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau2");
            }
            else if (movingCard.PilePosition == new Vector2(Layout.Tableau3.X, Layout.Tableau3.Y + deckManager.CardsInPlay["Tableau3"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau3");
            }
            else if (movingCard.PilePosition == new Vector2(Layout.Tableau4.X, Layout.Tableau4.Y + deckManager.CardsInPlay["Tableau4"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau4");
            }
            else if (movingCard.PilePosition == new Vector2(Layout.Tableau5.X, Layout.Tableau5.Y + deckManager.CardsInPlay["Tableau5"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau5");
            }
            else if (movingCard.PilePosition == new Vector2(Layout.Tableau6.X, Layout.Tableau6.Y + deckManager.CardsInPlay["Tableau6"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau6");
            }
            else if (movingCard.PilePosition == new Vector2(Layout.Tableau7.X, Layout.Tableau7.Y + deckManager.CardsInPlay["Tableau7"].Length * Layout.VerticalOffset))
            {
                deckManager.AddCardToPile(movingCard, "Tableau7");
            }
            movingCard = new Card();
        }
        public void MoveCard(MouseState mState, MouseState pmState, Vector2 smPosition, Vector2 psmPosition)
        {
            movingCard.Position = new Vector2(
                    movingCard.Position.X + smPosition.X - psmPosition.X,
                    movingCard.Position.Y + smPosition.Y - psmPosition.Y
                    );
            movingCard.CardBox = Layout.MakeCardBox(movingCard.Position);
        }
    }
}
