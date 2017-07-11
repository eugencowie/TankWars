using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TankWars
{
    /// <summary>
    /// The level contains all of the game objects and common code.
    /// </summary>
    sealed class Level
    {
        // Used for loading content.
        private ContentManager m_content;

        // Used to draw the score and high score.
        private SpriteFont m_font;

        // List of game objects.
        private List<IGameObject> m_objects;

        // High score.
        private string m_highScoreFile;
        private float m_highScore;
        private bool m_drawHighScore;

        // Player's score.
        private float m_score;

        public Level(ContentManager content, LevelData data, string highscoreFile)
        {
            m_content = content;

            m_font = content.Load<SpriteFont>("Game/Font");

            m_objects = new List<IGameObject>();

            // Create tanks.
            foreach (var tank in data.Tanks)
            {
                if (tank.Type == TankType.Player)
                {
                    m_objects.Add(new PlayerTank(this, m_content, tank.Position, (tank.Team == 0 ? Color.Blue : Color.Red), tank.Team));
                }
                else if (tank.Type == TankType.AI)
                {
                    m_objects.Add(new AITank(this, m_content, tank.Position, (tank.Team == 0 ? Color.Blue : Color.Red), tank.Team));
                }
            }

            // Create pickups.
            foreach (var pickup in data.Pickups)
            {
                if (pickup.Type == PickupType.Health)
                {
                    m_objects.Add(new HealthPickup(m_content, pickup.Position));
                }
                else if (pickup.Type == PickupType.Ammo)
                {
                    m_objects.Add(new AmmoPickup(m_content, pickup.Position));
                }
            }

            // Create obstacles.
            foreach (var obstacle in data.Obstacles)
            {
                m_objects.Add(new Obstacle(m_content, obstacle.Texture, obstacle.Position));
            }

            // Set initial high score values.
            m_highScoreFile = highscoreFile;
            m_highScore = 0;
            m_drawHighScore = false;

            // Set initial score.
            m_score = 0;

            // Load high score from file.
            if (File.Exists(highscoreFile))
            {
                string text = File.ReadAllText(highscoreFile);
                try { float.TryParse(text, out m_highScore); }
                catch { m_highScore = 0; }
            }
            else
            {
                m_highScore = 0;
            }
        }

        /// <summary>
        /// Returns all collidable game objects.
        /// </summary>
        public IEnumerable<ICollidable> Collidables
        {
            get { return m_objects.OfType<ICollidable>(); }
        }

        /// <summary>
        /// Returns all drawable game objects.
        /// </summary>
        public IEnumerable<IDrawable> Drawables
        {
            get { return m_objects.OfType<IDrawable>(); }
        }
        
        /// <summary>
        /// Returns all tanks.
        /// </summary>
        public IEnumerable<Tank> Tanks
        {
            get { return m_objects.OfType<Tank>(); }
        }

        /// <summary>
        /// Writes the high score to a file.
        /// </summary>
        public void WriteHighScore()
        {
            if (m_score > m_highScore)
            {
                m_highScore = m_score;
                File.WriteAllText(m_highScoreFile, m_highScore.ToString());
            }
        }

        /// <summary>
        /// Updates all objects in the level.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Update all updatable objects.
            foreach (var updatable in m_objects.OfType<IUpdatable>().ToList())
            {
                updatable.Update(gameTime);
            }

            // Check for collisions between all collidables.
            var collidables = m_objects.OfType<ICollidable>().ToList();
            for (int i=0; i<collidables.Count; i++)
            {
                for (int j=i+1; j<collidables.Count; j++)
                {
                    // Check for circle collisions.
                    if (collidables[j].Collider is CircleCollider)
                    {
                        if (collidables[i].Collider.Intersects((CircleCollider)collidables[j].Collider))
                        {
                            collidables[i].Collision(collidables[j]);
                            collidables[j].Collision(collidables[i]);
                        }
                    }
                    // Check for rectangle collisions.
                    else if (collidables[j].Collider is RectangleCollider)
                    {
                        if (collidables[i].Collider.Intersects((RectangleCollider)collidables[j].Collider))
                        {
                            collidables[i].Collision(collidables[j]);
                            collidables[j].Collision(collidables[i]);
                        }
                    }
                    else
                    {
                        throw new InvalidCastException("Attempted to use a collider other than CircleCollider or RectangleCollider.");
                    }
                }
            }

            // Remove any destroyed objects.
            foreach (var destroyable in m_objects.OfType<IDestroyable>().Where(d => d.Destroyed).ToList())
            {
                m_objects.Remove(destroyable);
            }

            // Update score only when there are both player and AI tanks on the screen.
            if (Tanks.OfType<PlayerTank>().Any() && Tanks.OfType<AITank>().Any())
            {
                m_score += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            m_drawHighScore = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all drawable objects.
            foreach (var drawable in m_objects.OfType<IDrawable>().OrderBy(d => d.Layer))
            {
                drawable.Draw(spriteBatch);
            }

            // Draw score and high score text.
            if (m_drawHighScore)
            {
                spriteBatch.DrawString(m_font, "High Score: " + new TimeSpan(0, 0, (int)m_highScore), new Vector2(40, 40), Color.White);
                spriteBatch.DrawString(m_font, "Score: " + new TimeSpan(0, 0, (int)m_score), new Vector2(40, 60), Color.White);
            }
        }

        /// <summary>
        /// Spawn a bullet.
        /// </summary>
        public void SpawnBullet(Vector2 position, Vector2 velocity, int team=0)
        {
            m_objects.Add(new Bullet(m_content, position, velocity, team));
        }

        /// <summary>
        /// Removes a game object from the level.
        /// </summary>
        public void DeleteObject(IGameObject obj)
        {
            m_objects.Remove(obj);
        }

        /// <summary>
        /// Convert the current level state to LevelData.
        /// </summary>
        public LevelData ToLevelData()
        {
            LevelData data = new LevelData();

            // Player tanks.
            foreach (var tank in m_objects.OfType<PlayerTank>())
            {
                data.Tanks.Add(new TankData(TankType.Player, tank.Team, tank.Position));
            }

            // AI tanks.
            foreach (var tank in m_objects.OfType<AITank>())
            {
                data.Tanks.Add(new TankData(TankType.AI, tank.Team, tank.Position));
            }

            // Health pickups.
            foreach (var pickup in m_objects.OfType<HealthPickup>())
            {
                data.Pickups.Add(new PickupData(PickupType.Health, pickup.Position));
            }

            // Ammo pickups.
            foreach (var pickup in m_objects.OfType<AmmoPickup>())
            {
                data.Pickups.Add(new PickupData(PickupType.Ammo, pickup.Position));
            }
            
            // Obstacles.
            foreach (var obstacle in m_objects.OfType<Obstacle>())
            {
                data.Obstacles.Add(new ObstacleData(obstacle.Texture.Name, obstacle.Position));
            }

            return data;
        }
    }
}
