using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;

namespace TankWars
{
    /// <summary>
    /// A tank where movement, aiming and firing is controlled by AI.
    /// </summary>
    sealed class AITank : Tank
    {
        private Vector2 m_direction;
        private static Random m_random;

        public AITank(Level level, ContentManager content, Vector2 position, Color color, int team=0)
            : base(level, content, position, color, team)
        {
            m_direction = Vector2.Zero;
            m_random = new Random();
        }
        
        /// <summary>
        /// Updates the tank.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Move(Vector2.Zero);
            
            // Get other tanks on the screen.

            var otherTanks = Level.Tanks.Where(tank => tank != this);
            if (otherTanks.Count() > 0)
            {
                // Move away from nearby tanks.

                foreach (var tank in otherTanks)
                {
                    Vector2 difference = tank.Position - Position;

                    if (difference.Length() > 0 && difference.Length() < 100)
                    {
                        m_direction += -Vector2.Normalize(difference);
                    }
                }

                // Get the closest enemy which is not on the same team.

                var enemyTanks = otherTanks.Where(tank => tank.Team != Team);
                if (enemyTanks.Count() > 0)
                {
                    Tank closestEnemy = enemyTanks.OrderBy(tank => (tank.Position - Position).LengthSquared()).First();

                    // Move toward closest tank.

                    if (closestEnemy != null)
                    {
                        Vector2 difference = closestEnemy.Position - Position;
                        if (difference.Length() > 120)
                        {
                            m_direction += Vector2.Normalize(difference);
                        }
                    }

                    // Aim at closest enemy.

                    if (closestEnemy != null)
                    {
                        AimAt(closestEnemy.Position);
                    }

                    // Fire at closest tank.

                    if (m_random.Next(150) < 1)
                    {
                        Fire();
                    }

                    // Apply movement.

                    if (m_direction.Length() > 0)
                    {
                        m_direction.Normalize();

                        float speed = m_random.Next(150, 250);

                        Move(m_direction * speed);
                    }
                }
            }
            
            base.Update(gameTime);
        }
    }
}
