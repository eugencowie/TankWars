using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TankWars
{
    /// <summary>
    /// A pickup which can be picked up.
    /// </summary>
    abstract class Pickup : IDrawable, ICollidable, IDestroyable
    {
        public int Layer { get; private set; }
        public bool Destroyed { get; protected set; }

        private Sprite m_sprite;

        private SoundEffect m_collected;

        public Pickup(ContentManager content, string texture, Vector2 position)
        {
            Layer = 5;

            m_sprite = new Sprite(content, texture, position);

            m_collected = content.Load<SoundEffect>("Game/Pickup");
        }

        /// <summary>
        /// ICollidable.Collider
        /// </summary>
        public ICollider Collider
        {
            get { return new CircleCollider(m_sprite.Position, Math.Max(m_sprite.Bounds.Width, m_sprite.Bounds.Height) / 2); }
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
        public virtual void Collision(ICollidable other)
        {
            if (other is Tank)
            {
                m_collected.Play();
            }
        }

        /// <summary>
        /// Draws the pickup.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            m_sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Draws the pickup.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            m_sprite.Draw(spriteBatch, color);
        }
    }
}
