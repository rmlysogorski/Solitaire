using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solitare.Managers;
using Solitare.Models;

namespace Solitare.States
{
    public class GameState : State
    {
        public static DeckManager deckManager;
        public static CardBack cardBack;
        public static MovingCardManager mcm;
        public static MovingStackManager msm;

        public static Texture2D background;
        public static SpriteFont gameFont;

        double clickTimer;

        MouseState mState;
        MouseState pmState;

        public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _content) : base(_game, _graphicsDevice, _content)
        {
            background = content.Load<Texture2D>("Images/Backgrounds/background2");
            gameFont = content.Load<SpriteFont>("gameFont");

            deckManager = new DeckManager();
            deckManager.MakeDeck(deckManager.CardsInPlay["Deck"], _content);
            deckManager.ShuffleCards(deckManager.CardsInPlay["Deck"]);

            cardBack = new CardBack()
            {
                SpriteTexture = content.Load<Texture2D>("Images/Cards/Backs/2")
            };

            deckManager.CreateFoundationPiles(_content);
            deckManager.PopulateTableaus();

            pmState = Mouse.GetState();

            clickTimer = 0;

            mcm = new MovingCardManager();

            msm = new MovingStackManager();
        }

        public override void Draw(GameTime gameTime, Renderer r)
        {
            r.Draw(gameTime, "game");
        }

        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();
            CollisionManager cm = new CollisionManager(mState, pmState);

            if(!mcm.MovingCard.IsMoving && !msm.StackIsMoving)
            {
                if(MouseInput.CheckForSingleClick(pmState)
                    && new Rectangle((int)Layout.PlayAgain.X, (int)Layout.PlayAgain.Y, 135, 30).Contains(mState.Position))
                {
                    game.ChangeToGameState();
                }

                deckManager.SendDoubleClicksToFoundation(mState, pmState, gameTime, clickTimer);

                deckManager.ClickToFlipTableauCard(mState, pmState);

                if (cm.DeckPileClick())
                {
                    if (deckManager.CardsInPlay["Deck"].Length <= 0)
                    {
                        deckManager.ReturnWasteToDeck();
                    }
                    else
                    {
                        if (deckManager.CardWasAlreadyDrawn)
                        {
                            deckManager.AddCardToPile(deckManager.CardsInPlay["Deck"][deckManager.CardsInPlay["Deck"].Length - 1], "Waste");
                            deckManager.CardsInPlay["Deck"] = deckManager.DownsizePile(deckManager.CardsInPlay["Deck"]);
                        }

                        if (deckManager.CardsInPlay["Deck"].Length > 0)
                        {
                            deckManager.DrawACard();
                        }
                        else
                        {
                            deckManager.CardWasAlreadyDrawn = false;
                        }
                    }
                }
            }            

            if (mcm.CheckIfMovementStopped(mState) || msm.CheckIfMovementStopped(mState))
            {
                if (mcm.MovingCard.IsMoving)
                {
                    mcm.MovingCard.IsMoving = false;
                    if (cm.CheckCollisions(mcm.MovingCard, deckManager))
                    {
                        mcm.MovingCard = new Card();
                    }
                    else
                    {
                        mcm.ReturnCardToPile(deckManager);
                    }
                }
                else
                {
                    msm.StackIsMoving = false;
                    if(cm.CheckStackCollisions(msm.MovingStack, deckManager))
                    {
                        msm.MovingStack = new Card[0];
                    }
                    else
                    {
                        msm.ReturnStackToPile(deckManager);
                    }
                }
            }

            if(!mcm.MovingCard.IsMoving && msm.CheckForMovement(deckManager, mState, pmState))
            {
                msm.MoveStack(mState, pmState);
            }

            if (!msm.StackIsMoving && mcm.CheckForMovement(deckManager, mState, pmState))
            {
                mcm.MoveCard(mState, pmState);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (MouseInput.CheckForDoubleClick(pmState, gameTime, clickTimer))
            {
                clickTimer = 0;
            }
            else if (MouseInput.CheckForSingleClick(pmState))
            {
                clickTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }
            pmState = mState;
        }
    }
}
