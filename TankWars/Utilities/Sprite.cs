using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    /// <summary>
    /// A drawable 2D object.
    /// </summary>
    class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }

        private Vector2 m_origin { get { return Texture.Bounds.Center.ToVector2(); } }

        public Sprite(ContentManager content, string texture, Vector2 position, Color color)
        {
            Texture = content.Load<Texture2D>(texture);
            Position = position;
            Rotation = 0;
            Color = color;
        }

        public Sprite(ContentManager content, string texture, Vector2 position)
            : this(content, texture, position, Color.White)
        {
        }

        /// <summary>
        /// Calculates the bounding box for the sprite.
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle((Position - m_origin).ToPoint(), Texture.Bounds.Size); }
        }

        /// <summary>
        /// Draws the sprite using the specified colour.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Color overrideColor)
        {
            spriteBatch.Draw(Texture, Position, null, overrideColor, Rotation, m_origin, Vector2.One, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws the sprute using the default colour.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color);
        }
    }
}
