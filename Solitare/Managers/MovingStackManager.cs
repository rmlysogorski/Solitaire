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
    public class MovingStackManager
    {
        private Card[] movingStack = new Card[0];
        private bool stackIsMoving = false;
        private string stackKey = string.Empty;
        public Card[] MovingStack { get => movingStack; set => movingStack = value; }
        public bool StackIsMoving { get => stackIsMoving; set => stackIsMoving = value; }

        public bool CheckForMovement(DeckManager deckManager, MouseState mState, MouseState pmState, Vector2 smPosition)
        {
            if (stackIsMoving)
            {
                return true;
            }
            else
            {
                if(mState.LeftButton == ButtonState.Pressed && pmState.LeftButton == ButtonState.Released)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        string key = "Tableau" + i;
                        if(deckManager.CardsInPlay[key].Length > 0)
                        {
                            for (int j = 0; j < deckManager.CardsInPlay[key].Length - 1; j++)
                            {
                                if (!deckManager.CardsInPlay[key][j].IsFaceDown)
                                {
                                    
                                    Rectangle cardbox = new Rectangle();
                                    cardbox.X = deckManager.CardsInPlay[key][j].CardBox.X;
                                    cardbox.Y = deckManager.CardsInPlay[key][j].CardBox.Y;
                                    cardbox.Width = deckManager.CardsInPlay[key][j].CardBox.Width;
                                    cardbox.Height = Layout.VerticalOffset;
                                    if (cardbox.Contains(smPosition))
                                    {
                                        //We have a hit so add this card and all subsequent cards to the movingStack Array
                                        //And also Remove them from this pile
                                        //Then return true
                                        Array.Resize(ref movingStack, deckManager.CardsInPlay[key].Length - j);
                                        for(int k = 0; k < deckManager.CardsInPlay[key].Length - j; k++)
                                        {
                                            movingStack[k] = deckManager.CardsInPlay[key][j + k];
                                        }
                                        Card[] temp = deckManager.CardsInPlay[key];
                                        Array.Resize(ref temp, deckManager.CardsInPlay[key].Length - movingStack.Length);
                                        deckManager.CardsInPlay[key] = temp;
                                        stackKey = key;
                                        stackIsMoving = true;
                                        return true;
                                    }                                   
                                }
                            }
                        }
                    }
                }                
            }
            return false;
        }

        public void MoveStack(MouseState mState, MouseState pmState, Vector2 smPosition, Vector2 psmPosition)
        {
            for(int i = 0; i < movingStack.Length; i++)
            {
                movingStack[i].Position = new Vector2(
                    movingStack[i].Position.X + smPosition.X - psmPosition.X,
                    movingStack[i].Position.Y + smPosition.Y - psmPosition.Y
                    );
                if(i == 0)
                {
                    movingStack[i].CardBox = Layout.MakeCardBox(movingStack[i].Position);
                }
                else
                {
                    movingStack[i].CardBox = new Rectangle();
                }
            }
        }

        public bool CheckIfMovementStopped(MouseState mState)
        {
            return (mState.LeftButton == ButtonState.Released && stackIsMoving == true);
        }

        public void ReturnStackToPile(DeckManager deckManager)
        {
            foreach(Card c in movingStack)
            {
                deckManager.AddCardToPile(c, stackKey);
            }
        }
    }
}
