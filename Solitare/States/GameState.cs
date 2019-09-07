using System;
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

        private int shadowOffset = 5;

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameState.background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            DrawButtons(spriteBatch);

            DrawPostit(spriteBatch);

            DrawCardBack(spriteBatch);

            //Always draws moving cards on top (last)
            if (CheckMovingToDrawPile())
            {
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch);
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
            }
            else if (CheckMovingToWastePile())
            {
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch);
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
            }
            else if (CheckMovingToHeartsPile())
            {
                DrawTableauPiles(spriteBatch);
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
            }
            else if (CheckMovingToSpadesPile())
            {
                DrawTableauPiles(spriteBatch);
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
            }
            else if (CheckMovingToDiamondsPile())
            {
                DrawTableauPiles(spriteBatch);
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
            }
            else if (CheckMovingToClubsPile())
            {
                DrawTableauPiles(spriteBatch);
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
            }
            else if (CheckMovingToTableau1())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch);
            }
            else if (CheckMovingToTableau2())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch,2);
            }
            else if (CheckMovingToTableau3())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch, 3);
            }
            else if (CheckMovingToTableau4())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch, 4);
            }
            else if (CheckMovingToTableau5())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch, 5);
            }
            else if (CheckMovingToTableau6())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch, 6);
            }
            else if (CheckMovingToTableau7())
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch, 7);
            }
            else if (GameState.mcm.MovingCard.IsMoving || GameState.msm.StackIsMoving)
            {
                DrawWinButton(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
            }
            else
            {
                DrawWinButton(spriteBatch);
                DrawMovingStack(spriteBatch);
                DrawMovingCard(spriteBatch);
                DrawDrawPile(spriteBatch);
                DrawWastePile(spriteBatch);
                DrawFoundationPiles(spriteBatch);
                DrawTableauPiles(spriteBatch);
            }
        }

        protected bool CheckMovingToDrawPile()
        {
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position != GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToWastePile()
        {
            if (GameState.deckManager.CardsInPlay["Waste"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Waste"][GameState.deckManager.CardsInPlay["Waste"].Length - 1].Position != GameState.deckManager.CardsInPlay["Waste"][GameState.deckManager.CardsInPlay["Waste"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToHeartsPile()
        {
            if (GameState.deckManager.CardsInPlay["Hearts"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Hearts"][GameState.deckManager.CardsInPlay["Hearts"].Length - 1].Position != GameState.deckManager.CardsInPlay["Hearts"][GameState.deckManager.CardsInPlay["Hearts"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToSpadesPile()
        {
            if (GameState.deckManager.CardsInPlay["Spades"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Spades"][GameState.deckManager.CardsInPlay["Spades"].Length - 1].Position != GameState.deckManager.CardsInPlay["Spades"][GameState.deckManager.CardsInPlay["Spades"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToDiamondsPile()
        {
            if (GameState.deckManager.CardsInPlay["Diamonds"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Diamonds"][GameState.deckManager.CardsInPlay["Diamonds"].Length - 1].Position != GameState.deckManager.CardsInPlay["Diamonds"][GameState.deckManager.CardsInPlay["Diamonds"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToClubsPile()
        {
            if (GameState.deckManager.CardsInPlay["Clubs"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Clubs"][GameState.deckManager.CardsInPlay["Clubs"].Length - 1].Position != GameState.deckManager.CardsInPlay["Clubs"][GameState.deckManager.CardsInPlay["Clubs"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau1()
        {
            if (GameState.deckManager.CardsInPlay["Tableau1"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau1"][GameState.deckManager.CardsInPlay["Tableau1"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau1"][GameState.deckManager.CardsInPlay["Tableau1"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau2()
        {
            if (GameState.deckManager.CardsInPlay["Tableau2"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau2"][GameState.deckManager.CardsInPlay["Tableau2"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau2"][GameState.deckManager.CardsInPlay["Tableau2"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau3()
        {
            if (GameState.deckManager.CardsInPlay["Tableau3"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau3"][GameState.deckManager.CardsInPlay["Tableau3"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau3"][GameState.deckManager.CardsInPlay["Tableau3"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau4()
        {
            if (GameState.deckManager.CardsInPlay["Tableau4"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau4"][GameState.deckManager.CardsInPlay["Tableau4"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau4"][GameState.deckManager.CardsInPlay["Tableau4"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau5()
        {
            if (GameState.deckManager.CardsInPlay["Tableau5"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau5"][GameState.deckManager.CardsInPlay["Tableau5"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau5"][GameState.deckManager.CardsInPlay["Tableau5"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau6()
        {
            if (GameState.deckManager.CardsInPlay["Tableau6"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau6"][GameState.deckManager.CardsInPlay["Tableau6"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau6"][GameState.deckManager.CardsInPlay["Tableau6"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau7()
        {
            if (GameState.deckManager.CardsInPlay["Tableau7"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau7"][GameState.deckManager.CardsInPlay["Tableau7"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau7"][GameState.deckManager.CardsInPlay["Tableau7"].Length - 1].PilePosition;
            }
            return false;
        }

        protected void DrawCardBack(SpriteBatch spriteBatch)
        {
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 1
                || (GameState.deckManager.CardsInPlay["Deck"].Length == 1 && !GameState.deckManager.CardWasAlreadyDrawn)
                || (GameState.mcm.MovingCard.IsMoving && GameState.mcm.MovingCard.PilePosition == Layout.DrawPile && GameState.deckManager.CardsInPlay["Deck"].Length == 1))
            {
                for (int i = 0;
                    (GameState.deckManager.CardWasAlreadyDrawn ?
                        i < GameState.deckManager.CardsInPlay["Deck"].Length - 1 :
                        i < GameState.deckManager.CardsInPlay["Deck"].Length);
                    i++)
                {
                    spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(Layout.Deck.X + i * 3 + shadowOffset,
                        Layout.Deck.Y + shadowOffset),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                    spriteBatch.Draw(
                        GameState.cardBack.SpriteTexture,
                        new Vector2(Layout.Deck.X + i * 3,
                        Layout.Deck.Y),
                        null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );
                }

            }
            else
            {
                spriteBatch.Draw(
                        GameState.cardShadow,
                        Layout.Deck,
                        null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );
            }
        }

        protected void DrawDrawPile(SpriteBatch spriteBatch)
        {
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 0
                && GameState.deckManager.CardWasAlreadyDrawn
                && GameState.mcm.MovingCard.PilePosition != Layout.DrawPile
                && GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].PilePosition == Layout.DrawPile)
            {
                spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(
                        GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position.X + shadowOffset,
                        GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position.Y + shadowOffset),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].SpriteTexture,
                    GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position,
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
            }
        }

        protected void DrawWastePile(SpriteBatch spriteBatch)
        {
            int wasteLen = GameState.deckManager.CardsInPlay["Waste"].Length;
            if (wasteLen > 0)
            {
                if (wasteLen > 1 && GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position != GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].PilePosition)
                {
                    spriteBatch.Draw(
                        GameState.cardShadow,
                        new Vector2(GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].Position.X + shadowOffset,
                        GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].Position.Y + shadowOffset),
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );

                    spriteBatch.Draw(
                        GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].SpriteTexture,
                        GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].Position,
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );
                }
                spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position.X + shadowOffset,
                    GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position.Y + shadowOffset),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].SpriteTexture,
                    GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position,
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            }
        }

        protected void DrawFoundationPiles(SpriteBatch spriteBatch)
        {
            if (GameState.deckManager.CardsInPlay["Hearts"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.HeartsPile.X + shadowOffset, Layout.HeartsPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }
            if (GameState.deckManager.CardsInPlay["Spades"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.SpadesPile.X + shadowOffset, Layout.SpadesPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }
            if (GameState.deckManager.CardsInPlay["Diamonds"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.DiamondsPile.X + shadowOffset, Layout.DiamondsPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }
            if (GameState.deckManager.CardsInPlay["Clubs"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.ClubsPile.X + shadowOffset, Layout.ClubsPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Hearts"][GameState.deckManager.CardsInPlay["Hearts"].Length - 1].SpriteTexture, Layout.HeartsPile, null,
                    (GameState.deckManager.CardsInPlay["Hearts"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Spades"][GameState.deckManager.CardsInPlay["Spades"].Length - 1].SpriteTexture, Layout.SpadesPile, null,
                    (GameState.deckManager.CardsInPlay["Spades"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Diamonds"][GameState.deckManager.CardsInPlay["Diamonds"].Length - 1].SpriteTexture, Layout.DiamondsPile, null,
                    (GameState.deckManager.CardsInPlay["Diamonds"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Clubs"][GameState.deckManager.CardsInPlay["Clubs"].Length - 1].SpriteTexture, Layout.ClubsPile, null,
                    (GameState.deckManager.CardsInPlay["Clubs"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

        }

        protected void DrawTableauPiles(SpriteBatch spriteBatch, int drawLast = 1)
        {
            for (int i = 0; i < 7; i++)
            {
                if (i + 1 != drawLast)
                {
                    int t = i + 1;
                    string key = "Tableau" + t;

                    DrawTableauPileShadow(spriteBatch, key);
                    DrawTableauPileCards(spriteBatch, key);
                }
            }

            DrawTableauPileShadow(spriteBatch, "Tableau" + drawLast);
            DrawTableauPileCards(spriteBatch, "Tableau" + drawLast);
        }

        protected void DrawTableauPileShadow(SpriteBatch spriteBatch, string key)
        {
            if (GameState.deckManager.CardsInPlay[key].Length <= 0
                || (GameState.deckManager.CardsInPlay[key].Length > 0
                    && GameState.deckManager.CardsInPlay[key][0].Position
                        != GameState.deckManager.CardsInPlay[key][0].PilePosition))
            {
                spriteBatch.Draw(GameState.cardShadow,
                    GetTableauPosition(key),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            }
        }

        protected void DrawTableauPileCards(SpriteBatch spriteBatch, string key)
        {
            foreach (Card c in GameState.deckManager.CardsInPlay[key])
            {
                spriteBatch.Draw(GameState.cardShadow, new Vector2(c.Position.X + shadowOffset, c.Position.Y + shadowOffset), null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);

                if (c.IsFaceDown)
                {
                    spriteBatch.Draw(GameState.cardBack.SpriteTexture, c.Position, null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(c.SpriteTexture, c.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                }
            }
        }

        protected void DrawButtons(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameState.playAgainButton, new Vector2(Layout.PlayAgain.X - Layout.ButtonRadius, Layout.PlayAgain.Y - Layout.ButtonRadius), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.playAgainButton, new Vector2(Layout.PlayAgain.X - Layout.ButtonRadius, Layout.PlayAgain.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.exitButton, new Vector2(Layout.Exit.X - Layout.ButtonRadius, Layout.Exit.Y - Layout.ButtonRadius), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.exitButton, new Vector2(Layout.Exit.X - Layout.ButtonRadius, Layout.Exit.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

        }

        protected void DrawPostit(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameState.postit, new Vector2(50, 86), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.46f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.postit, new Vector2(50, 80), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(GameState.pencil, "Score", Layout.Score, Color.Black);

            spriteBatch.DrawString(GameState.pencil, GameState.score.ToString(),
                new Vector2(Layout.PlayerScore.X - GameState.pencil.MeasureString(GameState.score.ToString()).X, Layout.PlayerScore.Y), Color.Black);

        }

        protected void DrawMovingStack(SpriteBatch spriteBatch)
        {
            if (GameState.msm.StackIsMoving)
            {
                foreach (Card c in GameState.msm.MovingStack)
                {
                    spriteBatch.Draw(GameState.cardShadow,
                        new Vector2(c.Position.X + shadowOffset * 2,
                        c.Position.Y + shadowOffset * 2),
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );

                    spriteBatch.Draw(c.SpriteTexture, c.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                }
            }
        }

        protected void DrawMovingCard(SpriteBatch spriteBatch)
        {
            if (GameState.mcm.MovingCard.IsMoving)
            {
                spriteBatch.Draw(GameState.cardShadow,
                    new Vector2(GameState.mcm.MovingCard.Position.X + shadowOffset * 2,
                    GameState.mcm.MovingCard.Position.Y + shadowOffset * 2),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(GameState.mcm.MovingCard.SpriteTexture, GameState.mcm.MovingCard.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
            }
        }

        protected void DrawWinButton(SpriteBatch spriteBatch)
        {
            if (GameState.canWin)
            {
                spriteBatch.DrawString(GameState.pencil, "You won!", new Vector2(800, 800), Color.Yellow);
            }
        }

        protected Vector2 GetTableauPosition(string key)
        {
            if (key == "Tableau1")
            {
                return Layout.Tableau1;
            }
            else if (key == "Tableau2")
            {
                return Layout.Tableau2;
            }
            else if (key == "Tableau3")
            {
                return Layout.Tableau3;
            }
            else if (key == "Tableau4")
            {
                return Layout.Tableau4;
            }
            else if (key == "Tableau5")
            {
                return Layout.Tableau5;
            }
            else if (key == "Tableau6")
            {
                return Layout.Tableau6;
            }
            else if (key == "Tableau7")
            {
                return Layout.Tableau7;
            }
            return Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            var scaleX = (float)graphicsDevice.PresentationParameters.BackBufferWidth / 1920;
            var scaleY = (float)graphicsDevice.PresentationParameters.BackBufferHeight / 1080;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            var pMousePosition = new Vector2(pmState.X, pmState.Y);
            var psmPosition = Vector2.Transform(pMousePosition, Matrix.Invert(matrix));

            mState = Mouse.GetState();
            var mousePosition = new Vector2(mState.X, mState.Y);
            var smPosition = Vector2.Transform(mousePosition, Matrix.Invert(matrix));

            CollisionManager cm = new CollisionManager(mState, pmState, smPosition);
            playAgainDist = Vector2.Distance(Layout.PlayAgain, new Vector2(smPosition.X, smPosition.Y));
            exitDist = Vector2.Distance(Layout.Exit, new Vector2(smPosition.X, smPosition.Y));

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
                    
                    deckManager.SendDoubleClicksToFoundation(mState, pmState, gameTime, clickTimer, smPosition);

                    deckManager.ClickToFlipTableauCard(mState, pmState, smPosition);

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

            if(!mcm.MovingCard.IsMoving && msm.CheckForMovement(deckManager, mState, pmState, smPosition))
            {
                msm.MoveStack(mState, pmState, smPosition, psmPosition);
            }

            if (!msm.StackIsMoving && mcm.CheckForMovement(deckManager, mState, pmState, smPosition))
            {
                mcm.MoveCard(mState, pmState, smPosition, psmPosition);
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
