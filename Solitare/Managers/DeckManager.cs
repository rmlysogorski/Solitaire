using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solitare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare.Managers
{
    public class DeckManager
    {
        private Dictionary<string, Card[]> cardsInPlay = new Dictionary<string, Card[]>()
        {
            {"Deck", new Card[52] },
            {"Waste", new Card[0] },
            {"Hearts", new Card[1] },
            {"Spades", new Card[1] },
            {"Diamonds", new Card[1] },
            {"Clubs", new Card[1] },
            {"Tableau1", new Card[0] },
            {"Tableau2", new Card[0] },
            {"Tableau3", new Card[0] },
            {"Tableau4", new Card[0] },
            {"Tableau5", new Card[0] },
            {"Tableau6", new Card[0] },
            {"Tableau7", new Card[0] },
        };

        private bool cardWasAlreadyDrawn;
        private static Random rand = new Random();
        
        public Dictionary<string, Card[]> CardsInPlay { get => cardsInPlay; set => cardsInPlay = value; }
        public bool CardWasAlreadyDrawn { get => cardWasAlreadyDrawn; set => cardWasAlreadyDrawn = value; }

        #region Deck Building Methods
        public void MakeDeck(Card[] deck, ContentManager content)
        {
            BuildSuit(deck, "S", 0, content);
            BuildSuit(deck, "H", 13, content);
            BuildSuit(deck, "C", 26, content);
            BuildSuit(deck, "D", 39, content);
        }

        public void BuildSuit(Card[] deck, string code, int indexStart, ContentManager content)
        {
            string path = GetPath(code);

            for (int i = indexStart; i < indexStart + 13; i++)
            {
                deck[i] = new Card(
                    content.Load<Texture2D>(
                        path + ((indexStart > 0) ? i % indexStart + 1 : i + 1).ToString()
                        ),
                    Layout.DrawPile,
                    code + ((indexStart > 0) ? i % indexStart + 1 : i + 1).ToString());
            }
        }

        public string GetPath(string code)
        {
            string path = string.Empty;
            switch (code)
            {
                case "S":
                    path = "Images/Cards/Spades/";
                    break;
                case "H":
                    path = "Images/Cards/Hearts/";
                    break;
                case "C":
                    path = "Images/Cards/Clubs/";
                    break;
                case "D":
                    path = "Images/Cards/Diamonds/";
                    break;
                default:
                    break;
            }
            return path;
        }

        public void ShuffleCards(Card[] deck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                Card temp = deck[i];
                int tempNum = rand.Next(0, 52);
                deck[i] = deck[tempNum];
                deck[tempNum] = temp;
            }
        }

        #endregion

        #region Foundation Methods

        public void CreateFoundationPiles(ContentManager content)
        {
            CardsInPlay["Hearts"][0] = new Card(content.Load<Texture2D>("Images/Cards/Hearts/1"),Layout.HeartsPile, "H0");
            CardsInPlay["Hearts"][0].PilePosition = Layout.HeartsPile;
            CardsInPlay["Hearts"][0].CardBox = Layout.MakeCardBox(Layout.HeartsPile);

            CardsInPlay["Spades"][0] = new Card(content.Load<Texture2D>("Images/Cards/Spades/1"), Layout.SpadesPile, "S0");
            CardsInPlay["Spades"][0].PilePosition = Layout.SpadesPile;
            CardsInPlay["Spades"][0].CardBox = Layout.MakeCardBox(Layout.SpadesPile);

            CardsInPlay["Diamonds"][0] = new Card(content.Load<Texture2D>("Images/Cards/Diamonds/1"), Layout.DiamondsPile, "D0");
            CardsInPlay["Diamonds"][0].PilePosition = Layout.DiamondsPile;
            CardsInPlay["Diamonds"][0].CardBox = Layout.MakeCardBox(Layout.DiamondsPile);

            CardsInPlay["Clubs"][0] = new Card(content.Load<Texture2D>("Images/Cards/Clubs/1"), Layout.ClubsPile, "C0");
            CardsInPlay["Clubs"][0].PilePosition = Layout.ClubsPile;
            CardsInPlay["Clubs"][0].CardBox = Layout.MakeCardBox(Layout.ClubsPile);
        }
        public string MakeOneCodeHigher(string code)
        {
            int num = int.Parse(code.Substring(1));
            num += 1;
            string oneHigher = code[0].ToString() + num;
            return oneHigher;
        }

        //public void AddToFoundationPile(Card card, string key)
        //{
        //    CardsInPlay[key] = ResizePile(CardsInPlay[key]);
        //    CardsInPlay[key][CardsInPlay[key].Length - 1] = DeterminePositionAndBox(card, key);
        //}

        //public Card DeterminePositionAndBox(Card card, string key)
        //{
        //    switch (key)
        //    {
        //        case "Hearts":
        //            card.Position = Layout.HeartsPile;
        //            card.PilePosition = Layout.HeartsPile;
        //            card.CardBox = Layout.MakeCardBox(Layout.HeartsPile);
        //            break;
        //        case "Spades":
        //            card.Position = Layout.SpadesPile;
        //            card.PilePosition = Layout.SpadesPile;
        //            card.CardBox = Layout.MakeCardBox(Layout.SpadesPile);
        //            break;
        //        case "Diamonds":
        //            card.Position = Layout.DiamondsPile;
        //            card.PilePosition = Layout.DiamondsPile;
        //            card.CardBox = Layout.MakeCardBox(Layout.DiamondsPile);
        //            break;
        //        case "Clubs":
        //            card.Position = Layout.ClubsPile;
        //            card.PilePosition = Layout.ClubsPile;
        //            card.CardBox = Layout.MakeCardBox(Layout.ClubsPile);
        //            break;
        //        default:
        //            break;
        //    }
        //    return card;
        //}

        public void SendDoubleClicksToFoundation(MouseState mState, MouseState pmState, GameTime gameTime, double clickTimer)
        {
            foreach (var keyValuePair in CardsInPlay)
            {
                if (keyValuePair.Value.Length > 0)
                {
                    Card thisCard = keyValuePair.Value[keyValuePair.Value.Length - 1];
                    if (thisCard.CardBox.Contains(mState.Position) && MouseInput.CheckForDoubleClick(pmState, gameTime, clickTimer))
                    {
                        if (thisCard.Code == MakeOneCodeHigher(CardsInPlay["Hearts"][CardsInPlay["Hearts"].Length - 1].Code)
                            || thisCard.Code == MakeOneCodeHigher(CardsInPlay["Spades"][CardsInPlay["Spades"].Length - 1].Code)
                            || thisCard.Code == MakeOneCodeHigher(CardsInPlay["Diamonds"][CardsInPlay["Diamonds"].Length - 1].Code)
                            || thisCard.Code == MakeOneCodeHigher(CardsInPlay["Clubs"][CardsInPlay["Clubs"].Length - 1].Code))
                        {
                            switch (thisCard.Code.First())
                            {
                                case 'H':  AddCardToPile(thisCard, "Hearts"); break;
                                case 'S':  AddCardToPile(thisCard, "Spades"); break;
                                case 'D':  AddCardToPile(thisCard, "Diamonds"); break;
                                case 'C':  AddCardToPile(thisCard, "Clubs"); break;
                                default:
                                    break;
                            }
                            CardsInPlay[keyValuePair.Key] = DownsizePile(CardsInPlay[keyValuePair.Key]);
                            if (keyValuePair.Key == "Deck")
                            {
                                CardWasAlreadyDrawn = false;
                            }
                            break;
                        }
                    }
                }
            }
        }       

        #endregion
        public Card[] DownsizePile(Card[] cardPile)
        {
            Card[] tempArray = cardPile;
            Array.Resize(ref tempArray, tempArray.Length - 1);
            return tempArray;
        }

        public Card[] ResizePile(Card[] cardPile)
        {
            Card[] tempArray = cardPile;
            Array.Resize(ref tempArray, tempArray.Length + 1);
            return tempArray;
        }

        //public void AddToWastePile(Card card)
        //{
        //    CardsInPlay["Waste"] = ResizePile(CardsInPlay["Waste"]);
        //    card.Position = Layout.WastePile;
        //    card.PilePosition = Layout.WastePile;
        //    card.CardBox = Layout.MakeCardBox(Layout.WastePile);
        //    CardsInPlay["Waste"][CardsInPlay["Waste"].Length - 1] = card;
        //}

        public void ReturnWasteToDeck()
        {
            CardsInPlay["Deck"] = CardsInPlay["Waste"];
            foreach (Card c in CardsInPlay["Deck"])
            {
                c.Position = Layout.DrawPile;
                c.PilePosition = new Vector2();
                c.CardBox = new Rectangle();
            }
            Array.Reverse(CardsInPlay["Deck"]);
            CardsInPlay["Waste"] = new Card[0];
        }

        public void DrawACard()
        {
            CardWasAlreadyDrawn = true;
            CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].Position = Layout.DrawPile;
            CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].PilePosition = Layout.DrawPile;
            CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].CardBox = Layout.MakeCardBox(Layout.DrawPile);
        }

        //public void AddToDeck(Card card)
        //{
        //    if(CardsInPlay["Deck"].Length > 0)
        //    {
        //        CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].PilePosition = new Vector2();
        //        CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].CardBox = new Rectangle();
        //    }
        //    card.Position = Layout.DrawPile;
        //    card.PilePosition = Layout.DrawPile;
        //    card.CardBox = Layout.MakeCardBox(Layout.DrawPile);
        //    CardsInPlay["Deck"] = ResizePile(CardsInPlay["Deck"]);
        //    CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1] = card;
        //}

        public void PopulateTableaus()
        {
            Card[] tempDeck = CardsInPlay["Deck"];
            Card[][] tempTableaus = new Card[7][];
            tempTableaus[0] = new Card[1];
            tempTableaus[1] = new Card[2];
            tempTableaus[2] = new Card[3];
            tempTableaus[3] = new Card[4];
            tempTableaus[4] = new Card[5];
            tempTableaus[5] = new Card[6];
            tempTableaus[6] = new Card[7];
            Vector2 tempPos = new Vector2();
            //Set loop to go through Foundation Piles Only
            for (int i = 0; i < 7; i++)
            {
                //if (keyValuePair.Key.StartsWith("T"))
                //{
                    //Set loop to start with 1 iteration and move up by one until it hits 7
                    for (int j = 0; j < tempTableaus[i].Length; j++)
                    {
                        //add a card from the deck
                        tempTableaus[i][j] = tempDeck[tempDeck.Length - 1];
                        Array.Resize(ref tempDeck, tempDeck.Length - 1);
                        switch (i)
                        {
                            case 0: tempPos = Layout.Tableau1;
                                break;
                            case 1: tempPos = Layout.Tableau2;
                                break;
                            case 2:tempPos = Layout.Tableau3;
                                break;
                            case 3:tempPos = Layout.Tableau4;
                                break;
                            case 4:tempPos = Layout.Tableau5;
                                break;
                            case 5:tempPos = Layout.Tableau6;
                                break;
                            case 6:tempPos = Layout.Tableau7;
                                break;
                            default:
                                break;
                        }
                        tempTableaus[i][j].Position = new Vector2(tempPos.X, tempPos.Y + (j * Layout.VerticalOffset));
                        tempTableaus[i][j].PilePosition = tempTableaus[i][j].Position;
                        tempTableaus[i][j].CardBox = Layout.MakeCardBox(tempTableaus[i][j].Position);
                         //Set all but the last card to face down
                        if (j < tempTableaus[i].Length - 1)
                            {
                                tempTableaus[i][j].IsFaceDown = true;
                            }
                    }
                //}
            }

            CardsInPlay["Deck"] = tempDeck;
            CardsInPlay["Tableau1"] = tempTableaus[0];
            CardsInPlay["Tableau2"] = tempTableaus[1];
            CardsInPlay["Tableau3"] = tempTableaus[2];
            CardsInPlay["Tableau4"] = tempTableaus[3];
            CardsInPlay["Tableau5"] = tempTableaus[4];
            CardsInPlay["Tableau6"] = tempTableaus[5];
            CardsInPlay["Tableau7"] = tempTableaus[6];
        }

        public void AddCardToPile(Card card, string key)
        {
            //thisCard.AnimateCardMovement(Layout.HeartsPile);
            Vector2 tempPos = new Vector2();
            switch (key)
            {
                case "Deck": tempPos = Layout.DrawPile; break;
                case "Waste": tempPos = Layout.WastePile; break;
                case "Hearts": tempPos = Layout.HeartsPile; break;
                case "Spades": tempPos = Layout.SpadesPile; break;
                case "Diamonds": tempPos = Layout.DiamondsPile; break;
                case "Clubs": tempPos = Layout.ClubsPile; break;
                case "Tableau1": tempPos = Layout.Tableau1; break;
                case "Tableau2": tempPos = Layout.Tableau2; break;
                case "Tableau3": tempPos = Layout.Tableau3; break;
                case "Tableau4": tempPos = Layout.Tableau4; break;
                case "Tableau5": tempPos = Layout.Tableau5; break;
                case "Tableau6": tempPos = Layout.Tableau6; break;
                case "Tableau7": tempPos = Layout.Tableau7; break;
            }
            
            if(key == "Waste" && card.PilePosition == Layout.DrawPile)
            {

                card.Position = tempPos;
                card.PilePosition = tempPos;
            }
            else
            {
                if (key == "Deck")
                {
                    if (CardsInPlay["Deck"].Length > 0)
                    {
                        CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].PilePosition = new Vector2();
                        CardsInPlay["Deck"][CardsInPlay["Deck"].Length - 1].CardBox = new Rectangle();
                    }
                    CardWasAlreadyDrawn = true;
                }

                if (key.StartsWith("T"))
                {
                    //card.Position = new Vector2(tempPos.X, tempPos.Y + (CardsInPlay[key].Length * Layout.VerticalOffset));
                    card.PilePosition = new Vector2(tempPos.X, tempPos.Y + (CardsInPlay[key].Length * Layout.VerticalOffset));
                }
                else
                {
                    //card.Position = tempPos;
                    card.PilePosition = tempPos;
                }
            }
            

            //card.PilePosition = tempPos;
            card.CardBox = Layout.MakeCardBox(card.PilePosition);

            CardsInPlay[key] = ResizePile(CardsInPlay[key]);
            CardsInPlay[key][CardsInPlay[key].Length - 1] = card;
        }

        public void ClickToFlipTableauCard(MouseState mState, MouseState pmState)
        {
            for (int i = 1; i < 8; i++)
            {
                string key = "Tableau" + i;
                if (CardsInPlay[key].Length > 0
                    && CardsInPlay[key][CardsInPlay[key].Length - 1].IsFaceDown
                    && MouseInput.CheckForSingleClick(pmState)
                    && CardsInPlay[key][CardsInPlay[key].Length - 1].CardBox.Contains(mState.Position))
                {
                    CardsInPlay[key][CardsInPlay[key].Length - 1].IsFaceDown = false;
                }
            }
        }

        public bool CanAddToTableauPile(Card card, Card[] tableau)
        {
            string thisCode = string.Empty;
            if (tableau.Length > 0)
            {
                if (card.Code.StartsWith("S") || card.Code.StartsWith("C"))
                {
                    if (tableau[tableau.Length - 1].Code.StartsWith("H") || tableau[tableau.Length - 1].Code.StartsWith("D"))
                    {
                        if (int.Parse(card.Code.Substring(1)) == (int.Parse(tableau[tableau.Length - 1].Code.Substring(1)) - 1))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (tableau[tableau.Length - 1].Code.StartsWith("S") || tableau[tableau.Length - 1].Code.StartsWith("C"))
                    {
                        if (int.Parse(card.Code.Substring(1)) == (int.Parse(tableau[tableau.Length - 1].Code.Substring(1)) - 1))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckForWin(MovingCardManager mcm, MovingStackManager msm)
        {
            if(CardsInPlay["Deck"].Length <= 0 
                && CardsInPlay["Waste"].Length <= 0
                && !mcm.MovingCard.IsMoving
                && !msm.StackIsMoving)
            {
                for(int i = 1; i < 8; i++)
                {
                    string key = "Tableau" + i;
                    foreach(Card c in CardsInPlay[key])
                    {
                        if (c.IsFaceDown)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }


        public void WinGame()
        {
            Card thisCard;
            for (int i = 1; i < 8; i++)
            {
                string key = "Tableau" + i;
                if (CardsInPlay[key].Length > 0)
                {
                    thisCard = CardsInPlay[key][CardsInPlay[key].Length - 1];
                    if (thisCard.Code == MakeOneCodeHigher(CardsInPlay["Hearts"][CardsInPlay["Hearts"].Length - 1].Code)
                            || thisCard.Code == MakeOneCodeHigher(CardsInPlay["Spades"][CardsInPlay["Spades"].Length - 1].Code)
                            || thisCard.Code == MakeOneCodeHigher(CardsInPlay["Diamonds"][CardsInPlay["Diamonds"].Length - 1].Code)
                            || thisCard.Code == MakeOneCodeHigher(CardsInPlay["Clubs"][CardsInPlay["Clubs"].Length - 1].Code))
                    {
                        switch (thisCard.Code.First())
                        {
                            case 'H': AddCardToPile(thisCard, "Hearts"); break;
                            case 'S': AddCardToPile(thisCard, "Spades"); break;
                            case 'D': AddCardToPile(thisCard, "Diamonds"); break;
                            case 'C': AddCardToPile(thisCard, "Clubs"); break;
                            default:
                                break;
                        }
                        CardsInPlay[key] = DownsizePile(CardsInPlay[key]);
                    }
                }
            }
        }
    }

}
