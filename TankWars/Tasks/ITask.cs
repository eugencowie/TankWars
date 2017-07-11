using Microsoft.Xna.Framework;

namespace TankWars
{
    /// <summary>
    /// Interface for a task.
    /// </summary>
    interface ITask
    {
        /// <summary>
        /// Indicates that the task has finished.
        /// </summary>
        bool Finished { get; }

        /// <summary>
        /// Called when the task should reset.
        /// </summary>
        void Reset();

        /// <summary>
        /// Called when the task should update.
        /// </summary>
        void Update(GameTime gameTime);
    }
}
