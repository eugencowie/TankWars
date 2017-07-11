using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TankWars
{
    [JsonConverter(typeof(StringEnumConverter))]
    enum TankType { Player, AI }

    /// <summary>
    /// Serializable data for a tank.
    /// </summary>
    sealed class TankData
    {
        /// <summary>
        /// The type of tank.
        /// </summary>
        public TankType Type { get; set; }

        /// <summary>
        /// The team that the tank is on.
        /// </summary>
        public int Team { get; set; }

        /// <summary>
        /// The position of the tank.
        /// </summary>
        public Vector2 Position { get; set; }

        public TankData(TankType type, int team, Vector2 position)
        {
            Type = type;
            Team = team;
            Position = position;
        }

        public TankData()
            : this(TankType.AI, 0, Vector2.Zero)
        {
        }
    }
}
