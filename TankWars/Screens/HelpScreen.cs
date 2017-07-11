using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWars
{
    /// <summary>
    /// The help screen shows the controls and instructions.
    /// </summary>
    sealed class HelpScreen : BaseScreen
    {
        private Texture2D m_helpTexture;
        private Button m_backButton;

        public HelpScreen(BaseScreen prev) : base(prev)
        {
            m_helpTexture = Content.Load<Texture2D>("Menu/Help");
            m_backButton = new Button(Content, "Menu/Buttons/Back", new Vector2(1131, 600));
        }

        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (m_backButton.IsClicked(Input) || Input.IsJustReleased(Keys.Escape))
            {
                Screens.Pop();
            }
        }

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(m_helpTexture, Vector2.Zero, Color.White);
            m_backButton.Draw(spriteBatch);

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
