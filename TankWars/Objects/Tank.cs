using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace TankWars
{
    /// <summary>
    /// A tank which can be moved and can fire bullets.
    /// </summary>
    abstract class Tank : IUpdatable, IDrawable, ICollidable, IDestroyable
    {
        public int Layer { get; private set; }
        public bool Destroyed { get; set; }

        public int Team { get; private set; }
        public int Health { get; set; }
        public int Ammo { get; set; }
        
        protected Level Level;

        private Sprite m_baseSprite;
        private Sprite m_turretSprite;

        private SoundEffect m_shoot;

        private float m_speed;
        private float m_angle;

        private float m_targetSpeed;
        private float m_targetAngle;

        private int m_turretCooldownTimer;

        public Tank(Level level, ContentManager content, Vector2 position, Color color, int team=0)
        {
            Layer = 20;
            Destroyed = false;
            Team = team;

            Level = level;

            m_baseSprite = new Sprite(content, "Game/Tank_Base", position, color);
            m_turretSprite = new Sprite(content, "Game/Tank_Turret", position, color);

            m_shoot = content.Load<SoundEffect>("Game/Shoot");

            m_speed = 0;
            m_angle = 0;

            m_targetSpeed = 0;
            m_targetAngle = 0;

            m_turretCooldownTimer = 0;
        }

        /// <summary>
        /// The color of the tank.
        /// </summary>
        protected Color Color
        {
            get { return m_baseSprite.Color; }
            set { m_baseSprite.Color = m_turretSprite.Color = value; }
        }

        /// <summary>
        /// ICollidable.Collider
        /// </summary>
        public ICollider Collider
        {
            get { return new CircleCollider(m_baseSprite.Position, Math.Max(m_baseSprite.Bounds.Width, m_baseSprite.Bounds.Height) / 2); }
        }

        /// <summary>
        /// IDrawable.Texture
        /// </summary>
        public Texture2D Texture
        {
            get { return m_baseSprite.Texture; }
            set { m_baseSprite.Texture = value; }
        }

        /// <summary>
        /// IDrawable.Position
        /// </summary>
        public Vector2 Position
        {
            get { return m_baseSprite.Position; }
            set { m_baseSprite.Position = m_turretSprite.Position = value; }
        }

        /// <summary>
        /// ICollidable.Collision
        /// </summary>
        public void Collision(ICollidable other)
        {
        }

        /// <summary>
        /// Updates the tank's movement.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            // Accelerate or decelerate until speed matches target speed.

            float accel = 800 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_speed < m_targetSpeed - accel)
            {
                m_speed += accel;
            }
            else if (m_speed > m_targetSpeed + accel)
            {
                m_speed -= accel;
            }
            else
            {
                m_speed = m_targetSpeed;
            }

            // Turn until angle matches target angle.

            float turnAccel = MathHelper.ToRadians(m_speed * 0.8f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            float diff = MathHelper.WrapAngle(m_targetAngle - m_angle);
            if (diff - turnAccel > 0)
            {
                m_angle += turnAccel;
            }
            else if (diff + turnAccel < 0)
            {
                m_angle -= turnAccel;
            }
            else
            {
                m_angle = m_targetAngle;
            }

            // Apply the movement.

            Vector2 forwardDirection = new Vector2((float)Math.Sin(m_angle), -(float)Math.Cos(m_angle));
            Vector2 velocity = forwardDirection * m_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (CanMove(velocity.X, 0))
            {
                Position = Position + new Vector2(velocity.X, 0);
            }

            if (CanMove(0, velocity.Y))
            {
                Position = Position + new Vector2(0, velocity.Y);
            }

            // Set turret rotation.
            
            m_baseSprite.Rotation = m_angle;

            // Decrease turret cooldown.

            if (m_turretCooldownTimer > 0)
            {
                m_turretCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        /// <summary>
        /// Check if it is possible to move in the specified direction without colliding with anything.
        /// </summary>
        private bool CanMove(Vector2 velocity)
        {
            CircleCollider newCollider = (CircleCollider)Collider;
            newCollider.Position += velocity;

            if (Level.Collidables.Any(c => c != this && (c is Tank || c is Obstacle) && c.Collider.Intersects(newCollider)))
            {
                return false;
            }

            var screenBounds = new Rectangle(0, 0, 1280, 720);
            if (!screenBounds.Contains(newCollider.GetRekt()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if it is possible to move in the specified direction without colliding with anything.
        /// </summary>
        private bool CanMove(float x, float y)
        {
            return CanMove(new Vector2(x, y));
        }

        /// <summary>
        /// Draws the tank.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, m_baseSprite.Color);
        }

        /// <summary>
        /// Draws the tank.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            m_baseSprite.Draw(spriteBatch, color);
            m_turretSprite.Draw(spriteBatch, color);
        }

        /// <summary>
        /// Moves the tank by the specified amount.
        /// </summary>
        protected void Move(Vector2 targetVelocity)
        {
            m_targetSpeed = targetVelocity.Length();

            if (m_targetSpeed > 0)
            {
                // Point the tank in the direction it is traveling.
                var normal = Vector2.Normalize(targetVelocity);
                m_targetAngle = (float)Math.Atan2(normal.X, -normal.Y);
            }
        }

        /// <summary>
        /// Aim in the specified direction.
        /// </summary>
        protected void Aim(Vector2 direction)
        {
            direction.Normalize();
            m_turretSprite.Rotation = (float)Math.Atan2(direction.X, -direction.Y);
        }

        /// <summary>
        /// Aim at the specified target position.
        /// </summary>
        protected void AimAt(Vector2 target)
        {
            Aim(target - m_baseSprite.Position);
        }

        /// <summary>
        /// Fire a bullet, if possible.
        /// </summary>
        protected void Fire()
        {
            if (m_turretCooldownTimer <= 0)
            {
                Vector2 direction = new Vector2((float)Math.Sin(m_turretSprite.Rotation), -(float)Math.Cos(m_turretSprite.Rotation));
                Level.SpawnBullet(Position, direction, Team);
                m_turretCooldownTimer = 250;

                m_shoot.Play();
            }
        }
    }
}
