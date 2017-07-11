using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    /// <summary>
    /// A immovable object which can be collided with.
    /// </summary>
    sealed class Obstacle : IDrawable, ICollidable
    {
        public int Layer { get; private set; }

        private Sprite m_sprite;

        public Obstacle(ContentManager content, string texture, Vector2 position)
        {
            Layer = 0;
            m_sprite = new Sprite(content, texture, position);
        }

        /// <summary>
        /// ICollidable.Collider
        /// </summary>
        public ICollider Collider
        {
            get { return new RectangleCollider(m_sprite.Bounds.Location.ToVector2(), m_sprite.Bounds.Size.ToVector2()); }
        }

        /// <summary>
        /// IDrawable.Texture
        /// </summary>
        public Texture2D Texture
        {
            get { return m_sprite.Texture; }
            set { m_sprite.Texture = value; }
        }

        /// <summary>
        /// IDrawable.Position
        /// </summary>
        public Vector2 Position
        {
            get { return m_sprite.Position; }
            set { m_sprite.Position = value; }
        }

        /// <summary>
        /// ICollidable.Collision
        /// </summary>
        public void Collision(ICollidable other)
        {
        }

        /// <summary>
        /// Draws the obstacle.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            m_sprite.Draw(spriteBatch);
            //Debug.Draw(spriteBatch, ((RectangleCollider)Collider).GetRekt(), Color.Blue);
        }

        /// <summary>
        /// Draws the obstacle.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            m_sprite.Draw(spriteBatch, color);
            //Debug.Draw(spriteBatch, ((RectangleCollider)Collider).GetRekt(), Color.Blue);
        }
    }
}
