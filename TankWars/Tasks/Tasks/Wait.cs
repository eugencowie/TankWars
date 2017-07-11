using Microsoft.Xna.Framework;
using System;

namespace TankWars.Tasks
{
    /// <summary>
    /// Waits for a condition to be true.
    /// </summary>
    sealed class Wait : ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// The condition to check.
        /// </summary>
        private readonly Predicate<float> m_condition;

        public Wait(Predicate<float> condition)
        {
            m_condition += condition;
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
            if (m_condition == null)
            {
                Finished = true;
            }
            else if (!Finished)
            {
                // Task is finished if the condition is true.
                Finished = m_condition((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}
