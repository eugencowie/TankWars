using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TankWars
{
    /// <summary>
    /// A 2D spritesheet animation.
    /// </summary>
    class Animation
    {
        // The spritesheet.
        private readonly Texture2D m_spritesheet;

        // Time per frame of animation, in seconds.
        private readonly float m_timePerFrame;

        /// Number of columns in the spritesheet.
        private readonly int m_columns;

        /// Number of rows in the spritesheet.
        private readonly int m_rows;

        /// The start frame of the animation (default: 0).
        private readonly int m_startFrame;

        /// The end frame of the animation (default: columns*rows-1).
        private readonly int m_endFrame;

        /// Whether the animation is looping/repeating.
        private readonly bool m_repeating;

        /// The width of each frame of animation (default: spritesheet_width / columns).
        private readonly int m_frameWidth;

        /// The height of each frame of animation (default: spritesheet_height / rows).
        private readonly int m_frameHeight;

        /// The current frame of animation.
        private int m_frame;

        /// Elapsed time since the start of the current frame.
        private float m_timeElapsed;

        public Animation(Texture2D spritesheet, float timePerFrame, int columns, int rows, int startFrame=0, int? endFrame=null, bool repeating=true)
        {
            m_spritesheet = spritesheet;
            m_timePerFrame = timePerFrame;

            m_columns = columns;
            m_rows = rows;

            m_startFrame = startFrame;
            m_endFrame = endFrame ?? (m_columns * m_rows) - 1;

            m_repeating = repeating;

            m_frameWidth = m_spritesheet.Width / m_columns;
            m_frameHeight = m_spritesheet.Height / m_rows;

            m_frame = 0;
            m_timeElapsed = 0;
        }

        /// <summary>
        /// The size of each frame of animation.
        /// </summary>
        public Point FrameSize
        {
            get { return new Point(m_frameWidth, m_frameHeight); }
        }

        /// <summary>
        /// Called when the animation should update.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            m_timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_timeElapsed > m_timePerFrame)
            {
                m_frame++;

                if (m_frame >= m_endFrame)
                {
                    m_frame = (m_repeating ? m_startFrame : m_endFrame);
                }

                m_timeElapsed -= m_timePerFrame;
            }
        }

        /// <summary>
        /// Called when the animation should draw.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
            int row = (int)Math.Floor((double)m_frame / m_columns);
            int column = m_frame - (row * m_columns);

            // The area of the spritesheet to draw.
            Rectangle sourceRect = new Rectangle(m_frameWidth * column, m_frameHeight * row, m_frameWidth, m_frameHeight);
            
            spriteBatch.Draw(m_spritesheet, position, sourceRect, Color.White, 0, Vector2.Zero, 1, effects, 0);
        }
    }
}
