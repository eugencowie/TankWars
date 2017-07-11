using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TankWars.Tasks
{
    /// <summary>
    /// Runs a series of tasks one after the other.
    /// </summary>
    sealed class Queue : ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// List of tasks to run.
        /// </summary>
        private readonly List<ITask> m_queue;

        /// <summary>
        /// Index of the current task being run.
        /// </summary>
        private int m_currentTask;

        public Queue(params ITask[] tasks)
        {
            m_queue = new List<ITask>(tasks);
            Reset();
        }

        /// <summary>
        /// Called when the task should reset.
        /// </summary>
        public void Reset()
        {
            Finished = false;

            // Reset all tasks.
            foreach (var task in m_queue)
            {
                task.Reset();
            }

            m_currentTask = 0;
        }

        /// <summary>
        /// Called when the task should update.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!Finished)
            {
                if (m_currentTask >= m_queue.Count)
                {
                    // If all tasks have been run, finish.
                    Finished = true;
                }
                else
                {
                    // Update the current task.
                    m_queue[m_currentTask].Update(gameTime);

                    // If current task is finished, move onto the next one.
                    if (m_queue[m_currentTask].Finished)
                    {
                        m_currentTask++;
                    }
                }
            }
        }
    }
}
