using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace TankWars
{
    /// <summary>
    /// The texture select screen allows the user to select a texture to apply to an object in the level editor.
    /// </summary>
    sealed class EditorTextureScreen : BaseScreen
    {
        // The action to take when a texture is selected.
        private event Action<string> m_onTextureSelected;

        // Dictionary containing possible textures to choose from.
        private Dictionary<string, Texture2D> m_textures;

        // Background texture.
        private Texture2D m_background;

        public EditorTextureScreen(BaseScreen prev, Action<string> onTextureSelected) : base(prev)
        {
            m_onTextureSelected = onTextureSelected;

            m_textures = new Dictionary<string, Texture2D>();

            m_background = Content.Load<Texture2D>("Menu/EditorBackground");

            FindTextures();
        }

        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Vector2 position = new Vector2(50, 50);
            foreach (var texture in m_textures)
            {
                Rectangle rect = texture.Value.Bounds;
                rect.Location += position.ToPoint();

                // Check if texture is selected.
                if (m_onTextureSelected != null && rect.Contains(Mouse.GetState().Position) && Input.IsJustReleased(MouseButtons.Left))
                {
                    m_onTextureSelected(texture.Key);
                    Screens.Pop();
                    break;
                }

                position.X += texture.Value.Width;
            }
        }

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            spriteBatch.Draw(m_background, GraphicsDevice.Viewport.Bounds, Color.White);

            // Draw textures.
            Vector2 position = new Vector2(50, 50);
            foreach (var texture in m_textures)
            {
                spriteBatch.Draw(texture.Value, position, Color.White);
                position.X += texture.Value.Width;
            }

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

        /// <summary>
        /// 
        /// </summary>
        private void FindTextures()
        {
            string searchPath = Path.Combine(Content.RootDirectory, "Game");

            foreach (var path in Directory.EnumerateFiles(searchPath))
            {
                // Set file name.
                string filename = path;

                // Remove file extension.
                filename = filename.Split('.')[0];
                
                // Remove content root directory.
                filename = filename.Remove(0, Content.RootDirectory.Length + 1);

                Texture2D texture = null;

                // try to load texture
                try { texture = Content.Load<Texture2D>(filename); }
                catch { continue; }

                if (texture != null)
                {
                    // add texture to list of available textures
                    m_textures[filename] = texture;
                }
            }
        }
    }
}
