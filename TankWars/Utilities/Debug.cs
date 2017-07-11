using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankWars
{
    static class Debug
    {
        private static Texture2D m_debugPixel = null;

        /// <summary>
        /// Draws a semi-transparent rectangle.
        /// </summary>
        public static void Draw(SpriteBatch spriteBatch, Rectangle rect, Color? color = null)
        {
            if (m_debugPixel == null)
            {
                m_debugPixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                Color[] data = new Color[1];
                data[0] = new Color(Color.White, 127);
                m_debugPixel.SetData(data);
            }

            spriteBatch.Draw(m_debugPixel, rect, color ?? Color.Red);
        }
    }
}
