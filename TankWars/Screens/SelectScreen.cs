using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TankWars
{
    /// <summary>
    /// The level select screen, where players can select a level to play or edit.
    /// </summary>
    sealed class SelectScreen : BaseScreen
    {
        // This reference to the menu screen is used to show or hide it when appropriate.
        private MenuScreen m_menu;

        // The action to take when a level is selected.
        private event Action<string> m_onLevelSelected;

        // Buttons for the different levels and back button.
        private Button[] m_levelButtons;
        private Button m_backButton;

        // Has the screen been covered by another screen.
        private bool m_visible;

        public SelectScreen(MenuScreen prev, Action<string> onLevelSelected) : base(prev)
        {
            m_menu = prev;

            m_onLevelSelected += onLevelSelected;

            m_levelButtons = new Button[] {
                new Button(Content, "Menu/Buttons/Level1", new Vector2(190, 600)),
                new Button(Content, "Menu/Buttons/Level2", new Vector2(390, 600)),
                new Button(Content, "Menu/Buttons/Level3", new Vector2(590, 600)),
                new Button(Content, "Menu/Buttons/Level4", new Vector2(790, 600)),
                new Button(Content, "Menu/Buttons/Level5", new Vector2(990, 600))
            };

            m_backButton = new Button(Content, "Menu/Buttons/Back", new Vector2(1131, 600));

            m_visible = true;
        }

        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (m_visible)
            {
                // Check if back button is clicked.
                if (m_backButton.IsClicked(Input) || Input.IsJustReleased(Keys.Escape))
                {
                    Screens.Pop();
                }
                else
                {
                    for (int i = 0; i < m_levelButtons.Length; i++)
                    {
                        // Check if level button is clicked.
                        if (m_onLevelSelected != null && m_levelButtons[i].IsClicked(Input))
                        {
                            string level = string.Format("Level{0}", i + 1);
                            m_onLevelSelected(level);
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
            if (m_visible)
            {
                spriteBatch.Begin();

                // Draw level buttons.
                foreach (var button in m_levelButtons)
                {
                    button.Draw(spriteBatch);
                }

                // Draw back button.
                m_backButton.Draw(spriteBatch);

                spriteBatch.End();
            }
        }

        /// <summary>
        /// Called when the screen is covered by another screen.
        /// </summary>
        public override void Covered(IScreen other)
        {
            m_visible = false;
            m_menu.Covered(other);
        }

        /// <summary>
        /// Called when the screen is uncovered (after another screen is removed).
        /// </summary>
        public override void Uncovered(IScreen other)
        {
            m_visible = true;
            m_menu.Uncovered(other);
        }
    }
}
