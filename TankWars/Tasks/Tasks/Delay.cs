using Microsoft.Xna.Framework;

namespace TankWars.Tasks
{
    /// <summary>
    /// Waits for a specified amount of time.
    /// </summary>
    sealed class Delay : ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// The time to wait, in seconds.
        /// </summary>
        private readonly float m_delay;

        /// <summary>
        /// The time which has elapsed so far, in seconds.
        /// </summary>
        private float m_elapsedTime;

        public Delay(float delay)
        {
            m_delay = delay;
            Reset();
        }

        /// <summary>
        /// Called when the task should reset.
        /// </summary>
        public void Reset()
        {
            Finished = false;
            m_elapsedTime = 0;
        }

        /// <summary>
        /// Called when the task should update.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!Finished)
            {
                // Increase elapsed time.
                m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if the specified wait time has passed.
                if (m_elapsedTime >= m_delay)
                {
                    Finished = true;
                }
            }
        }
    }
}
