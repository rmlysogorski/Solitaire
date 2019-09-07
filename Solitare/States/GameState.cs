﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public static Texture2D cardShadow;
        public static SpriteFont gameFont;

        public static Texture2D playAgainButton;
        public static Texture2D exitButton;

        public static Texture2D postit;
        public static SpriteFont pencil;
        public static int score;

        float playAgainDist;
        float exitDist;

        SoundEffect selectMenuItem;
        SoundEffect draw;
        SoundEffect returnToDeck;
        SoundEffect placeCard;
        bool smiPlayed;

        double clickTimer;

        public static bool canWin;

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

            cardShadow = content.Load<Texture2D>("Images/Cards/cardShadow");

            playAgainButton = content.Load<Texture2D>("Images/MenuItems/playAgainButton");
            exitButton = content.Load<Texture2D>("Images/MenuItems/exitButton");

            postit = content.Load<Texture2D>("Images/MenuItems/postit");
            pencil = content.Load<SpriteFont>("pencil");
            score = 0;

            deckManager.CreateFoundationPiles(_content);
            deckManager.PopulateTableaus();

            selectMenuItem = content.Load<SoundEffect>("Sounds/SoundEffects/selectMenuItem");
            draw = content.Load<SoundEffect>("Sounds/SoundEffects/draw");
            returnToDeck = content.Load<SoundEffect>("Sounds/SoundEffects/returnToDeck");
            placeCard = content.Load<SoundEffect>("Sounds/SoundEffects/placeCard");
            smiPlayed = false;

            pmState = Mouse.GetState();

            clickTimer = 0;

            canWin = false;

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
            playAgainDist = Vector2.Distance(Layout.PlayAgain, new Vector2(mState.X, mState.Y));
            exitDist = Vector2.Distance(Layout.Exit, new Vector2(mState.X, mState.Y));

            if (!mcm.MovingCard.IsMoving && !msm.StackIsMoving)
            {
                if (playAgainDist < Layout.ButtonRadius
                || exitDist < Layout.ButtonRadius)
                {
                    if (!smiPlayed)
                    {
                        PlaySoundEffect(selectMenuItem, 0.2f, 0.3f);
                        smiPlayed = true;
                    }
                }

                if (MouseInput.CheckForSingleClick(pmState))
                {
                    if (playAgainDist < Layout.ButtonRadius)
                    {
                        game.ChangeToGameState();
                    }
                    if (exitDist < Layout.ButtonRadius)
                    {
                        game.Exit();
                    }
                }

                if (!canWin)
                {
                    
                    deckManager.SendDoubleClicksToFoundation(mState, pmState, gameTime, clickTimer);

                    deckManager.ClickToFlipTableauCard(mState, pmState);

                    if (cm.DeckPileClick(deckManager))
                    {
                        if (deckManager.CardsInPlay["Deck"].Length <= 0)
                        {
                            deckManager.ReturnWasteToDeck();
                            PlaySoundEffect(returnToDeck, 0.2f, -0.5f);
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

                            PlaySoundEffect(draw, 0.3f, 0.8f);
                        }
                    }
                }
                else
                {
                    deckManager.WinGame();
                    if(deckManager.CardsInPlay["Hearts"].Length > 13
                        && deckManager.CardsInPlay["Spades"].Length > 13
                        && deckManager.CardsInPlay["Diamonds"].Length > 13
                        && deckManager.CardsInPlay["Clubs"].Length > 13)
                    {
                        canWin = false;
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

                PlaySoundEffect(placeCard, 0.1f, 0.6f);
            }

            if(!mcm.MovingCard.IsMoving && msm.CheckForMovement(deckManager, mState, pmState))
            {
                msm.MoveStack(mState, pmState);
            }

            if (!msm.StackIsMoving && mcm.CheckForMovement(deckManager, mState, pmState))
            {
                mcm.MoveCard(mState, pmState);
            }

            CheckCardsForAnimations(gameTime);

            if (!canWin)
            {
                canWin = deckManager.CheckForWin(mcm, msm);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            int hearts = deckManager.CardsInPlay["Hearts"].Length - 1;
            int spades = deckManager.CardsInPlay["Spades"].Length - 1;
            int diamonds = deckManager.CardsInPlay["Diamonds"].Length - 1;
            int clubs = deckManager.CardsInPlay["Clubs"].Length - 1;

            if (MouseInput.CheckForDoubleClick(pmState, gameTime, clickTimer))
            {
                clickTimer = 0;
            }
            else if (MouseInput.CheckForSingleClick(pmState))
            {
                clickTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (smiPlayed
                && playAgainDist > Layout.ButtonRadius
                && exitDist > Layout.ButtonRadius)
            {
                smiPlayed = false;
            }
            score = hearts + spades + diamonds + clubs;
            score *= 10;
            if(hearts > 12)
            {
                score += 120;
            }
            if (spades > 12)
            {
                score += 120;
            }
            if (diamonds > 12)
            {
                score += 120;
            }
            if (clubs > 12)
            {
                score += 120;
            }

            pmState = mState;
        }

        public void CheckCardsForAnimations(GameTime gameTime)
        {
            foreach(var kvp in deckManager.CardsInPlay)
            {
                if(kvp.Value.Length > 0)
                {
                    foreach(Card c in kvp.Value)
                    {
                        c.AnimateCardMovement(gameTime);
                    }
                }
            }
        }

        public void PlaySoundEffect(SoundEffect se, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            SoundEffectInstance sei = se.CreateInstance();
            sei.Volume = volume;
            sei.Pitch = pitch;
            sei.Pan = pan;
            sei.Play();
        }
    }
}
