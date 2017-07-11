using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TankWars.Tasks
{
    /// <summary>
    /// Runs a series of tasks concurrently.
    /// </summary>
    sealed class Concurrent : ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// List of tasks to run concurrently.
        /// </summary>
        private readonly List<ITask> m_tasks;

        /// <summary>
        /// How many tasks should finish before this task finishes (all tasks must finish if negative).
        /// </summary>
        private readonly int m_finishAfter;

        /// <summary>
        /// The current number of finished tasks.
        /// </summary>
        private int m_tasksFinished;

        public Concurrent(params ITask[] tasks)
            : this(-1, tasks)
        {
        }

        public Concurrent(int finishAfter, params ITask[] tasks)
        {
            m_tasks = new List<ITask>(tasks);
            m_finishAfter = finishAfter;
            Reset();
        }

        /// <summary>
        /// Called when the task should reset.
        /// </summary>
        public void Reset()
        {
            Finished = false;

            // Reset all tasks.
            foreach (var task in m_tasks)
            {
                task.Reset();
            }

            m_tasksFinished = 0;
        }

        /// <summary>
        /// Called when the task should update.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!Finished)
            {
                // Update all unfinished tasks.
                foreach (var task in m_tasks.Where(t => !t.Finished))
                {
                    task.Update(gameTime);

                    if (task.Finished)
                    {
                        m_tasksFinished++;
                    }
                }

                // If all tasks have finished then we are done.
                if (m_tasksFinished >= m_tasks.Count)
                {
                    Finished = true;
                }
                // If enough tasks have finished then we are done.
                else if (m_finishAfter > 0 && m_tasksFinished >= m_finishAfter)
                {
                    Finished = true;
                }
            }
        }
    }
}
