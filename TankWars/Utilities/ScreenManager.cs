using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TankWars
{
    /// <summary>
    /// Defines the interface for a game screen.
    /// </summary>
    interface IScreen
    {
        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Called when the screen is covered by another screen.
        /// </summary>
        void Covered(IScreen other);

        /// <summary>
        /// Called when the screen is uncovered (after another screen is removed).
        /// </summary>
        void Uncovered(IScreen other);
    }

    /// <summary>
    /// Manages a stack of screens, where screens can be layered on top of each other.
    /// </summary>
    sealed class ScreenManager
    {
        // List of the currently activate screens.
        private List<IScreen> m_screens;

        // A copy of the screen list to iterate over.
        private List<IScreen> m_copy;

        // Indicates if the copy needs updating.
        private bool m_invalidateCopy;

        public ScreenManager()
        {
            m_screens = new List<IScreen>();
            m_copy = null;
            m_invalidateCopy = true;
        }

        ~ScreenManager()
        {
            Clear();
        }

        /// <summary>
        /// Returns true if there are no screens.
        /// </summary>
        public bool Empty()
        {
            return !m_screens.Any();
        }

        /// <summary>
        /// Removes all screens.
        /// </summary>
        public void Clear()
        {
            while (!Empty())
            {
                Pop();
            }
        }

        /// <summary>
        /// Removes all existing screens and then adds the specified screen.
        /// </summary>
        public void SwitchTo(IScreen screen)
        {
            Clear();

            Push(screen);
        }

        /// <summary>
        /// Adds a new screen on top of the current screen.
        /// </summary>
        public void Push(IScreen screen)
        {
            if (m_screens.Any())
            {
                m_screens.Last().Covered(screen);
            }
            
            m_screens.Add(screen);
            m_invalidateCopy = true;
        }

        /// <summary>
        /// Removes the top-most screen.
        /// </summary>
        public void Pop()
        {
            if (!Empty())
            {
                IScreen removed = m_screens.Last();

                m_screens.Remove(removed);
                m_invalidateCopy = true;

                if (m_screens.Any())
                {
                    m_screens.Last().Uncovered(removed);
                }
            }
        }

        /// <summary>
        /// Updates all screens, from bottom-most to top-most.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            UpdateLists();

            foreach (var screen in m_copy)
            {
                screen.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all screens, from bottom-most to top-most.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateLists();

            foreach (var screen in m_copy)
            {
                screen.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Makes a new copy of the screen list if it has been changed.
        /// </summary>
        private void UpdateLists()
        {
            if (m_invalidateCopy)
            {
                m_copy = m_screens.ToList();
                m_invalidateCopy = false;
            }
        }
    }
}
