using System;
using TankWars.Tasks;

namespace TankWars
{
    /// <summary>
    /// This doesn't have much use for this game, but I got bored one evening so ¯\_(ツ)_/¯
    /// </summary>
    static class Task
    {
        //
        // These functions provide a convenient way to create tasks.
        //


        // Performs an action.
        public static Do Do(Action<float> action) { return new Do(action); }

        // Waits for a specified amount of time.
        public static Delay Delay(float delay) { return new Delay(delay); }

        //  Waits for a condition to be true.
        public static Wait Wait(Predicate<float> condition) { return new Wait(condition); }

        // Repeats a task a number of times.
        public static Repeat Repeat(ITask task) { return new Repeat(task); }
        public static Repeat Repeat(int times, ITask task) { return new Repeat(times, task); }

        // Runs a series of tasks one after the other.
        public static Queue Queue(params ITask[] tasks) { return new Queue(tasks); }

        // Runs a series of tasks concurrently.
        public static Concurrent Concurrent(params ITask[] tasks) { return new Concurrent(tasks); }
        public static Concurrent Concurrent(int finishAfter, params ITask[] tasks) { return new Concurrent(finishAfter, tasks); }


        //
        // These functions combine tasks to create more complex behaviour.
        //


        // Waits for a specified amount of time, then performs an action.
        public static Queue DelayDo(float delay, Action<float> action) { return Queue(Delay(delay), Do(action)); }

        // Repeats an action a number of times.
        public static Repeat RepeatDo(Action<float> action) { return Repeat(Do(action)); }
        public static Repeat RepeatDo(int times, Action<float> action) { return Repeat(times, Do(action)); }

        /// <summary>
        /// Repeats an ction for a certain amount of time.
        /// </summary>
        public static Concurrent DoFor(float forTime, Action<float> action)
        {
            // This Concurrent task will finish when at least one child task finishes. The
            // infinite Repeat task will never finish, but the Delay task will finish when
            // the specified time has elapsed, therefore the Concurrent task will also
            // finish when the specified time has elapsed.

            return Concurrent(1,
                Repeat(Do(action)),
                Delay(forTime)
            );
        }

        /// <summary>
        /// Repeats an action until a specified condition is met.
        /// </summary>
        public static Concurrent DoUntil(Action<float> action, Predicate<float> condition)
        {
            // This Concurrent task will finish when at least one child task finishes. The
            // infinite Repeat task will never finish but the Wait task *will* finish when
            // the condition is met, therefore the Concurrent task will *also* finish when
            // the condition is met.

            return Concurrent(1,
                Repeat(Do(action)),
                Wait(condition)
            );
        }
    }
}

/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TankWars
{
    class TankWarsGame : Game
    {
        private SpriteBatch m_spriteBatch;
        private Color m_background;

        private Texture2D m_texture;
        private Vector2 m_position;

        private List<ITask> m_tasks;

        public TankWarsGame()
        {
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                IsFullScreen = false
            };

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_background = Color.Red;

            m_texture = Content.Load<Texture2D>("Game/Tank_Base");
            m_position = Vector2.Zero;

            m_tasks = new List<ITask>
            {
                Task.Repeat(
                    Task.Queue(
                        Task.Delay(5), Task.Do(dt => ChangeBackground(Color.Green)),
                        Task.Delay(5), Task.Do(dt => ChangeBackground(Color.Blue)),
                        Task.Delay(5), Task.Do(dt => ChangeBackground(Color.Red))
                    )
                ),

                Task.Repeat(
                    Task.Queue(
                        Task.DoUntil(dt => MoveAction(dt, 100, 100), dt => HasReached(dt, 100, 100)),
                        Task.Delay(0.5f), Task.DoUntil(dt => MoveAction(dt, 500, 100), dt => HasReached(dt, 500, 100)),
                        Task.Delay(0.5f), Task.DoUntil(dt => MoveAction(dt, 500, 500), dt => HasReached(dt, 500, 500)),
                        Task.Delay(0.5f), Task.DoUntil(dt => MoveAction(dt, 100, 500), dt => HasReached(dt, 100, 500)),
                        Task.Delay(0.5f), Task.DoUntil(dt => MoveAction(dt, 300, 300), dt => HasReached(dt, 300, 300))
                    )
                )
            };
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var task in m_tasks)
            {
                task.Update(gameTime);
            }

            m_tasks.RemoveAll(task => task.Finished);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(m_background);

            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_texture, m_position, Color.White);
            m_spriteBatch.End();
            
            base.Draw(gameTime);
        }
        
        private void ChangeBackground(Color color)
        {
            m_background = color;
        }

        private void MoveAction(float dt, float x, float y)
        {
            m_position += Vector2.Normalize(new Vector2(x, y) - m_position) * 400 * dt;
        }

        private bool HasReached(float dt, float x, float y)
        {
            return ((new Vector2(x, y) - m_position).Length() < 400 * dt);
        }
    }
}
*/
