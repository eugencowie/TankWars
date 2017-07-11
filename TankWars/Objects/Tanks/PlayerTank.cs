using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TankWars
{
    /// <summary>
    /// A tank where movement, aiming and firing is controller by player input.
    /// </summary>
    sealed class PlayerTank : Tank
    {
        public PlayerTank(Level level, ContentManager content, Vector2 position, Color color, int team=0)
            : base(level, content, position, color, team)
        {
        }
        
        /// <summary>
        /// Updates the tank.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            // Tank movement.

            Vector2 velocity = Vector2.Zero;

            if (kb.IsKeyDown(Keys.W))
                velocity.Y--;

            if (kb.IsKeyDown(Keys.S))
                velocity.Y++;

            if (kb.IsKeyDown(Keys.A))
                velocity.X--;

            if (kb.IsKeyDown(Keys.D))
                velocity.X++;

            velocity.Normalize();

            if (velocity.Length() > 0)
            {
                Move(velocity * 300);
            }
            else
            {
                Move(Vector2.Zero);
            }

            // Turret rotation.

            AimAt(mouse.Position.ToVector2());

            // Fire turret.
            
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Fire();
            }
            
            base.Update(gameTime);
        }
    }
}
