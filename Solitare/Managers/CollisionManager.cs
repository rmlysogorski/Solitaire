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
    public class CollisionManager
    {
        private MouseState mState;
        private MouseState pmState;

        public CollisionManager(MouseState m, MouseState p)
        {
            mState = m;
            pmState = p;
        }
        //Checks for Mouse Click on Deck Pile
        public bool DeckPileClick(DeckManager deckManager)
        {
            return (MouseInput.CheckForSingleClick(pmState) && Layout.MakeCardBox(
                new Vector2(Layout.Deck.X + deckManager.CardsInPlay["Deck"].Length * 3 - 6, Layout.Deck.Y))
                .Contains(mState.Position));
        }
        public bool CheckCollisions(Card movingCard, DeckManager deckManager)
        {
            if (WastePileCollision(movingCard))
            {
                deckManager.AddCardToPile(movingCard, "Waste");
                return true;
            }
            else if (HeartsPileCollision(movingCard)
                && (movingCard.Code == deckManager.MakeOneCodeHigher(deckManager.CardsInPlay["Hearts"][deckManager.CardsInPlay["Hearts"].Length - 1].Code)))
            {                
                deckManager.AddCardToPile(movingCard, "Hearts");
                return true;               
            }
            else if (SpadesPileCollision(movingCard)
                && (movingCard.Code == deckManager.MakeOneCodeHigher(deckManager.CardsInPlay["Spades"][deckManager.CardsInPlay["Spades"].Length - 1].Code)))
            {
                deckManager.AddCardToPile(movingCard, "Spades");
                return true;
            }
            else if (DiamondsPileCollision(movingCard)
                && (movingCard.Code == deckManager.MakeOneCodeHigher(deckManager.CardsInPlay["Diamonds"][deckManager.CardsInPlay["Diamonds"].Length - 1].Code)))
            {
                deckManager.AddCardToPile(movingCard, "Diamonds");
                return true;
            }
            else if (ClubsPileCollision(movingCard)
                && (movingCard.Code == deckManager.MakeOneCodeHigher(deckManager.CardsInPlay["Clubs"][deckManager.CardsInPlay["Clubs"].Length - 1].Code)))
            {
                deckManager.AddCardToPile(movingCard, "Clubs");
                return true;
            }
            else
            {
                for(int i = 1; i < 8; i++)
                {
                    string key = "Tableau" + i;
                    if(deckManager.CardsInPlay[key].Length <= 0)
                    {
                        if(int.Parse(movingCard.Code.Substring(1)) == 13
                            && CheckEmptyTableauCollision(movingCard, key))
                        {
                            deckManager.AddCardToPile(movingCard, key);
                            return true;
                        }
                    }
                    else if(TableauPileCollision(movingCard, deckManager.CardsInPlay[key]))
                    {
                        if (deckManager.CanAddToTableauPile(movingCard, deckManager.CardsInPlay[key]))
                        {
                            deckManager.AddCardToPile(movingCard, key);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckStackCollisions(Card[] movingStack, DeckManager deckManager)
        {
            for(int i = 1; i < 8; i++)
            {
                string key = "Tableau" + i;
                if(deckManager.CardsInPlay[key].Length <= 0)
                {
                    if(int.Parse(movingStack[0].Code.Substring(1)) == 13
                        && CheckEmptyTableauCollision(movingStack[0], key))
                    {
                        foreach (Card c in movingStack)
                        {
                            deckManager.AddCardToPile(c, key);
                        }
                        return true;
                    }
                }
                else
                {
                    if(TableauPileCollision(movingStack[0], deckManager.CardsInPlay[key])
                        && deckManager.CanAddToTableauPile(movingStack[0], deckManager.CardsInPlay[key]))
                    {
                        foreach(Card c in movingStack)
                        {
                            deckManager.AddCardToPile(c, key);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public bool WastePileCollision(Card movingCard)
        {
            return (movingCard.PilePosition == Layout.DrawPile && movingCard.CardBox.Intersects(Layout.MakeCardBox(Layout.WastePile)));
        }
        public bool HeartsPileCollision(Card movingCard)
        {
            return (movingCard.CardBox.Intersects(Layout.MakeCardBox(Layout.HeartsPile)));
        }
        public bool SpadesPileCollision(Card movingCard)
        {
            return (movingCard.CardBox.Intersects(Layout.MakeCardBox(Layout.SpadesPile)));
        }
        public bool DiamondsPileCollision(Card movingCard)
        {
            return (movingCard.CardBox.Intersects(Layout.MakeCardBox(Layout.DiamondsPile)));
        }
        public bool ClubsPileCollision(Card movingCard)
        {
            return (movingCard.CardBox.Intersects(Layout.MakeCardBox(Layout.ClubsPile)));
        }

        public bool TableauPileCollision(Card movingCard, Card[] tableau)
        {
            return (movingCard.CardBox.Intersects(tableau[tableau.Length - 1].CardBox));
        }

        public bool CheckEmptyTableauCollision(Card card, string key)
        {
            Vector2 tempPos = new Vector2();
            switch (key)
            {
                case "Tableau1": tempPos = Layout.Tableau1; break;
                case "Tableau2": tempPos = Layout.Tableau2; break;
                case "Tableau3": tempPos = Layout.Tableau3; break;
                case "Tableau4": tempPos = Layout.Tableau4; break;
                case "Tableau5": tempPos = Layout.Tableau5; break;
                case "Tableau6": tempPos = Layout.Tableau6; break;
                case "Tableau7": tempPos = Layout.Tableau7; break;
            }
            return (card.CardBox.Intersects(Layout.MakeCardBox(tempPos)));
        }
    }
}
