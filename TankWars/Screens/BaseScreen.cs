using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    /// <summary>
    /// Base screen which all other screens inherit from.
    /// </summary>
    abstract class BaseScreen : IScreen
    {
        protected GraphicsDevice GraphicsDevice;
        protected ContentManager Content;
        protected ScreenManager Screens;
        protected InputManager Input;

        public BaseScreen(GraphicsDevice graphicsDevice, ContentManager content, ScreenManager screens, InputManager input)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
            Screens = screens;
            Input = input;
        }
        
        /// <summary>
        /// Instantiate the screen by getting the required variables from another screen.
        /// </summary>
        public BaseScreen(BaseScreen other)
            : this(other.GraphicsDevice, other.Content, other.Screens, other.Input)
        {
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Covered(IScreen other);
        public abstract void Uncovered(IScreen other);
    }
}
