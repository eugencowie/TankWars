using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWars
{
    /// <summary>
    /// The game screen, where the gameplay happens.
    /// </summary>
    sealed class GameScreen : BaseScreen
    {
        // The level contains all of the game objects.
        private Level m_level;
        
        public GameScreen(BaseScreen prev, string level) : base(prev)
        {
            m_level = new Level(Content, LevelData.Load(level), level + "_Score.txt");
        }

        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Update the level.
            m_level.Update(gameTime);

            if (Input.IsJustReleased(Keys.Escape))
            {
                // Write high score before going back to the level select screen.
                m_level.WriteHighScore();
                Screens.Pop();
                return;
            }
        }

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw the level.
            m_level.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Called when the screen is covered by another screen.
        /// </summary>
        public override void Covered(IScreen other)
        {
        }

        /// <summary>
        /// Called when the screen is uncovered (after another screen is removed).
        /// </summary>
        public override void Uncovered(IScreen other)
        {
        }
    }
}
