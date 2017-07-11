using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TankWars
{
    /// <summary>
    /// A pickup which increases the ammo of the tank which collects it.
    /// </summary>
    sealed class AmmoPickup : Pickup
    {
        public AmmoPickup(ContentManager content, Vector2 position)
            : base(content, "Game/Pickup_Ammo", position)
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
                tank.Ammo += 50;
                Destroyed = true;
            }

            base.Collision(other);
        }
    }
}
