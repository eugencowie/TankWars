using Microsoft.Xna.Framework;

namespace TankWars.Tasks
{
    /// <summary>
    /// Repeats a task a number of times.
    /// </summary>
    sealed class Repeat : ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// The task to repeat.
        /// </summary>
        private readonly ITask m_task;

        /// <summary>
        /// The number of times to repeat the task (repeats infinitely if negative number).
        /// </summary>
        private readonly int m_times;

        /// <summary>
        /// The current number of completed task repeats.
        /// </summary>
        private int m_current;

        public Repeat(ITask task)
            : this(-1, task)
        {
        }

        public Repeat(int times, ITask task)
        {
            m_task = task;
            m_times = times;
            Reset();
        }

        /// <summary>
        /// Called when the task should reset.
        /// </summary>
        public void Reset()
        {
            Finished = false;
            m_task.Reset();
            m_current = 0;
        }

        /// <summary>
        /// Called when the task should update.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!Finished)
            {
                // Update task.
                m_task.Update(gameTime);

                // When task finishes, reset it.
                if (m_task.Finished)
                {
                    m_task.Reset();
                    m_current++;
                }

                // If task has been reset enough times, finish.
                if (m_times > 0 && m_current >= m_times)
                {
                    Finished = true;
                }
            }
        }
    }
}
