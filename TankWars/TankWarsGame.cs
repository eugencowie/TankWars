using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    /// <summary>
    /// The main game class.
    /// </summary>
    class TankWarsGame : Game
    {
        // Used for drawing 2D graphics.
        private SpriteBatch m_spriteBatch;

        // Used for managing multiple stacked screens.
        private ScreenManager m_screens;

        // Used for managing gamepad, keyboard and mouse input.
        private InputManager m_input;

        public TankWarsGame()
        {
            // Specify the desired window properties.
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                IsFullScreen = false
            };

            // Make the mouse visible.
            IsMouseVisible = true;

            // Set the content directory.
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Initialise variables.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_screens = new ScreenManager();
            m_input = new InputManager();

            // Start by showing the menu screen.
            m_screens.Push(new MenuScreen(GraphicsDevice, Content, m_screens, m_input));
        }

        protected override void Update(GameTime gameTime)
        {
            // Update input and screen manager.
            m_input.Update();
            m_screens.Update(gameTime);

            // If all screens have been removed, exit the game.
            if (m_screens.Empty())
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the screens.
            m_screens.Draw(m_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
