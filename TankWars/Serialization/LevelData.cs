using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TankWars
{
    /// <summary>
    /// Serializable data for a level.
    /// </summary>
    sealed class LevelData
    {
        /// <summary>
        /// List of tanks in the level.
        /// </summary>
        public List<TankData> Tanks { get; set; }

        /// <summary>
        /// List of pickups in the level.
        /// </summary>
        public List<PickupData> Pickups { get; set; }

        /// <summary>
        /// List of obstacles in the level.
        /// </summary>
        public List<ObstacleData> Obstacles { get; set; }

        public LevelData(List<TankData> tanks, List<PickupData> pickups, List<ObstacleData> obstacles)
        {
            Tanks = tanks;
            Pickups = pickups;
            Obstacles = obstacles;
        }

        public LevelData()
            : this(new List<TankData>(), new List<PickupData>(), new List<ObstacleData>())
        {
        }

        /// <summary>
        /// Load level date from file.
        /// </summary>
        public static LevelData Load(string levelName)
        {
            string[] paths = {
                "Levels",
                "../../../../Levels"
            };
            string jsonData = string.Empty;

            for (int i = 0; i < paths.Length; i++)
            {
                string filename = Path.Combine(paths[i], levelName + ".json");

                if (File.Exists(filename))
                {
                    jsonData = File.ReadAllText(filename);
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                throw new FileNotFoundException("Levels/" + levelName + ".json");
            }

            return JsonConvert.DeserializeObject<LevelData>(jsonData);
        }

        /// <summary>
        /// Save level date to file.
        /// </summary>
        public static void Save(string levelName, LevelData data)
        {
            string[] paths = {
                "Levels",
                "../../../../Levels"
            };
            string destFile = string.Empty;

            for (int i = 0; i < paths.Length; i++)
            {
                string filename = Path.Combine(paths[i], levelName + ".json");

                if (File.Exists(filename))
                {
                    destFile = filename;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(destFile))
            {
                throw new FileNotFoundException("Levels/" + levelName + ".json");
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(destFile, jsonData);
            }
        }
    }
}
