using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TankWars
{
    [JsonConverter(typeof(StringEnumConverter))]
    enum PickupType { Health, Ammo }

    /// <summary>
    /// Serializable data for a pickup.
    /// </summary>
    sealed class PickupData
    {
        /// <summary>
        /// The type of pickup.
        /// </summary>
        public PickupType Type { get; set; }

        /// <summary>
        /// The posiiton of the pickup.
        /// </summary>
        public Vector2 Position { get; set; }

        public PickupData(PickupType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }
}
