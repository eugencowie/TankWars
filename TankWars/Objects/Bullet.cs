using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    /// <summary>
    /// A moving projectile that gets destroyed if it hits anything.
    /// </summary>
    sealed class Bullet : IUpdatable, IDrawable, ICollidable, IDestroyable
    {
        public int Layer { get; private set; }
        public bool Destroyed { get; private set; }

        private Sprite m_sprite;
        private Vector2 m_velocity;
        private int m_team;

        public Bullet(ContentManager content, Vector2 position, Vector2 velocity, int team=0)
        {
            Layer = 10;
            Destroyed = false;

            m_sprite = new Sprite(content, "Game/Bullet", position);
            m_velocity = velocity;
            m_team = team;
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
        /// Updates the bullet.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            m_sprite.Position += m_velocity * gameTime.ElapsedGameTime.Milliseconds;
            
            var screenBounds = new Rectangle(0, 0, 1280, 720);
            if (!screenBounds.Intersects(m_sprite.Bounds))
            {
                Destroyed = true;
            }
        }

        /// <summary>
        /// ICollidable.Collision
        /// </summary>
        public void Collision(ICollidable other)
        {
            if (other is Tank)
            {
                var tank = (Tank)other;
                if (tank.Team != m_team)
                {
                    tank.Destroyed = true;
                    Destroyed = true;
                }
            }
            else if (other is Obstacle)
            {
                Destroyed = true;
            }
        }

        /// <summary>
        /// Draws the bullet.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            m_sprite.Draw(spriteBatch);
            //Debug.Draw(spriteBatch, ((RectangleCollider)Collider).GetRekt(), Color.Red);
        }

        /// <summary>
        /// Draws the bullet.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            m_sprite.Draw(spriteBatch, color);
            //Debug.Draw(spriteBatch, ((RectangleCollider)Collider).GetRekt(), Color.Red);
        }
    }
}
