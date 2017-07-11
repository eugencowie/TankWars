using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWars
{
    /// <summary>
    /// The edit object screen allows the user to edit objects in the level editor.
    /// </summary>
    sealed class EditorObjectScreen : BaseScreen
    {
        // The level being edited.
        private Level m_level;

        // The object being edited.
        private IDrawable m_selected;
        
        // The action buttons.
        private Button m_deleteButton;
        private Button m_textureButton;
        
        // Has the screen been covered by another screen.
        private bool m_visible;

        public EditorObjectScreen(BaseScreen prev, Level level, IDrawable selected) : base(prev)
        {
            m_level = level;
            m_selected = selected;
            
            float posY = GraphicsDevice.Viewport.Height - 40;
            m_deleteButton = new Button(Content, "Menu/Buttons/Remove", new Vector2(100, posY));
            m_textureButton = new Button(Content, "Menu/Buttons/SetTexture", new Vector2(300, posY));

            m_visible = true;
        }

        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (m_visible)
            {
                if (Input.IsJustReleased(Keys.Escape))
                {
                    Screens.Pop();
                    return;
                }

                if (m_deleteButton.IsClicked(Input))
                {
                    // Delete selected object and return to the level editor.
                    m_level.DeleteObject(m_selected);
                    Screens.Pop();
                    return;
                }

                if (m_textureButton.IsClicked(Input))
                {
                    // Load the texture select screen. When a texture is selected, set the texture of the selected object.
                    Screens.Push(new EditorTextureScreen(this, textureName => {
                        m_selected.Texture = Content.Load<Texture2D>(textureName);
                    }));
                }
            }
        }

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_visible)
            {
                spriteBatch.Begin();

                // Draw the selected object in yellow, if it is drawable.
                m_selected.Draw(spriteBatch, Color.Yellow);

                // Draw the buttons.
                m_deleteButton.Draw(spriteBatch);
                m_textureButton.Draw(spriteBatch);

                spriteBatch.End();
            }
        }

        /// <summary>
        /// Called when the screen is covered by another screen.
        /// </summary>
        public override void Covered(IScreen other)
        {
            m_visible = false;
        }

        /// <summary>
        /// Called when the screen is uncovered (after another screen is removed).
        /// </summary>
        public override void Uncovered(IScreen other)
        {
            m_visible = true;
        }
    }
}
