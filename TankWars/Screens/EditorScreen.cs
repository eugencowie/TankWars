using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWars
{
    /// <summary>
    /// The level editor screen allows the user to edit levels.
    /// </summary>
    sealed class EditorScreen : BaseScreen
    {
        // The level being edited.
        private Level m_level;
        private string m_levelName;

        // The object being edited or moved.
        private IDrawable m_selected;
        private bool m_dragging;

        // Has the screen been covered by another screen.
        private bool m_visible;

        public EditorScreen(BaseScreen prev, string level) : base(prev)
        {
            // Load the level to edit.
            m_level = new Level(Content, LevelData.Load(level), level + "_Score.txt");
            m_levelName = level;
            
            // Initialise variables to default values.
            m_selected = null;
            m_dragging = false;
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
                    // Save the level before returning to the level select screen.
                    LevelData.Save(m_levelName, m_level.ToLevelData());
                    Screens.Pop();
                    return;
                }

                if (m_dragging)
                {
                    // Set the position of the dragged object to the mouse position.
                    m_selected.Position = Mouse.GetState().Position.ToVector2();

                    // Check if if mouse button is released (no longer dragging).
                    if (Input.IsJustReleased(MouseButtons.Left))
                    {
                        m_dragging = false;
                        m_selected = null;
                    }
                }
                else
                {
                    if (Input.IsJustPressed(MouseButtons.Left))
                    {
                        // Get the object which the mouse is over.
                        m_selected = GetSelected();

                        // If an object is selected, begin dragging.
                        if (m_selected != null)
                        {
                            m_dragging = true;
                        }
                    }

                    if (Input.IsJustReleased(MouseButtons.Right))
                    {
                        // Get the object which the mouse is over.
                        m_selected = GetSelected();

                        // If an object is selected, lload the edit object screen.
                        if (m_selected != null)
                        {
                            Screens.Push(new EditorObjectScreen(this, m_level, m_selected));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            m_level.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Called when the screen is covered by another screen.
        /// </summary>
        public override void Covered(IScreen other)
        {
            m_visible = false;
            m_selected = null;
        }

        /// <summary>
        /// Called when the screen is uncovered (after another screen is removed).
        /// </summary>
        public override void Uncovered(IScreen other)
        {
            m_visible = true;
            m_selected = null;
        }

        /// <summary>
        /// Gets the object (if any) that the mouse is over. If none found, returns null.
        /// </summary>
        private IDrawable GetSelected()
        {
            IDrawable selected = null;

            foreach (var obj in m_level.Collidables)
            {
                if (obj is IDrawable && obj.Collider.Contains(Mouse.GetState().Position))
                {
                    selected = (IDrawable)obj;
                    break;
                }
            }

            return selected;
        }
    }
}
