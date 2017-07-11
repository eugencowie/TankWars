using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TankWars
{
    /// <summary>
    /// A pickup which increases the ammo of the tank which collects it.
    /// </summary>
    sealed class HealthPickup : Pickup
    {
        public HealthPickup(ContentManager content, Vector2 position)
            : base(content, "Game/Pickup_Health", position)
        {
        }

        /// <summary>
        /// ICollider.Collision
        /// </summary>
        public override void Collision(ICollidable other)
        {
            if (other is Tank)
            {
                var tank = (Tank)other;
                tank.Health += 50;
                Destroyed = true;
            }

            base.Collision(other);
        }
    }
}
