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
        public bool CheckForMovement(DeckManager deckManager, MouseState mState, MouseState pmState)
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
                        && keyValuePair.Value[keyValuePair.Value.Length- 1].CardBox.Contains(mState.Position)
                        && !keyValuePair.Value[keyValuePair.Value.Length - 1].IsFaceDown
                        && mState.LeftButton == ButtonState.Pressed)
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
            else if(movingCard.PilePosition == Layout.Tableau1)
            {
                deckManager.AddCardToPile(movingCard, "Tableau1");
            }
            else if (movingCard.PilePosition == Layout.Tableau2)
            {
                deckManager.AddCardToPile(movingCard, "Tableau2");
            }
            else if (movingCard.PilePosition == Layout.Tableau3)
            {
                deckManager.AddCardToPile(movingCard, "Tableau3");
            }
            else if (movingCard.PilePosition == Layout.Tableau4)
            {
                deckManager.AddCardToPile(movingCard, "Tableau4");
            }
            else if (movingCard.PilePosition == Layout.Tableau5)
            {
                deckManager.AddCardToPile(movingCard, "Tableau5");
            }
            else if (movingCard.PilePosition == Layout.Tableau6)
            {
                deckManager.AddCardToPile(movingCard, "Tableau6");
            }
            else if (movingCard.PilePosition == Layout.Tableau7)
            {
                deckManager.AddCardToPile(movingCard, "Tableau7");
            }
            movingCard = new Card();
        }
        public void MoveCard(MouseState mState, MouseState pmState)
        {
            movingCard.Position = new Vector2(
                    movingCard.Position.X + mState.Position.X - pmState.Position.X,
                    movingCard.Position.Y + mState.Position.Y - pmState.Position.Y
                    );
            movingCard.CardBox = Layout.MakeCardBox(movingCard.Position);
        }
    }
}
