using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    /// <summary>
    /// Interface for any game object in the level.
    /// </summary>
    interface IGameObject
    {
    }

    /// <summary>
    /// Interface for game objects which can be updated.
    /// </summary>
    interface IUpdatable : IGameObject
    {
        void Update(GameTime gameTime);
    }

    /// <summary>
    /// Interface for game objects which can be drawn.
    /// </summary>
    interface IDrawable : IGameObject
    {
        Texture2D Texture { get; set; }
        Vector2 Position { get; set; }

        int Layer { get; }

        void Draw(SpriteBatch spriteBatch);
        void Draw(SpriteBatch spriteBatch, Color color);
    }

    /// <summary>
    /// Interface for game objects which can be destroyed.
    /// </summary>
    interface IDestroyable : IGameObject
    {
        bool Destroyed { get; }
    }

    /// <summary>
    /// Interface for game objects which can collide with each other.
    /// </summary>
    interface ICollidable : IGameObject
    {
        ICollider Collider { get; }

        /// <summary>
        /// Called when colliding with another object.
        /// </summary>
        void Collision(ICollidable other);
    }
}
