using Microsoft.Xna.Framework;

namespace TankWars
{
    /// <summary>
    /// Serializable data for an obstacle.
    /// </summary>
    sealed class ObstacleData
    {
        /// <summary>
        /// The obstacle texture.
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// The position of the obstacle.
        /// </summary>
        public Vector2 Position { get; set; }

        public ObstacleData(string texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public ObstacleData()
            : this(string.Empty, Vector2.Zero)
        {
        }
    }
}
