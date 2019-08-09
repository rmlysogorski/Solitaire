using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solitare.Models
{
    public class Card : Component
    {
        private Texture2D spriteTexture;
        private Vector2 position;
        private Vector2 pilePosition;
        private Rectangle cardBox;
        private bool isFaceDown;
        private bool isMoving;
        private string code;

        public Texture2D SpriteTexture { get => spriteTexture; set => spriteTexture = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 PilePosition { get => pilePosition; set => pilePosition = value; }
        public Rectangle CardBox { get => cardBox; set => cardBox = value; }
        public bool IsFaceDown { get => isFaceDown; set => isFaceDown = value; }
        public bool IsMoving { get => isMoving; set => isMoving = value; }
        public string Code { get => code; set => code = value; }

        public Card()
        {
            isFaceDown = false;
            isMoving = false;
        }
        public Card(Texture2D _spriteTexture, Vector2 _position, string _code)
        {
            spriteTexture = _spriteTexture;
            position = _position;
            code = _code;
            isFaceDown = false;
            isMoving = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
