using Microsoft.Xna.Framework;
using System;

namespace TankWars.Tasks
{
    /// <summary>
    /// Performs an action.
    /// </summary>
    sealed class Do : ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// The action to be performed.
        /// </summary>
        private readonly Action<float> m_action;

        public Do(Action<float> action)
        {
            m_action += action;
            Reset();
        }

        /// <summary>
        /// Called when the task should reset.
        /// </summary>
        public void Reset()
        {
            Finished = false;
        }

        /// <summary>
        /// Called when the task should update.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (m_action == null)
            {
                Finished = true;
            }
            else if (!Finished)
            {
                // Perform the action.
                m_action((float)gameTime.ElapsedGameTime.TotalSeconds);
                Finished = true;
            }
        }
    }
}
