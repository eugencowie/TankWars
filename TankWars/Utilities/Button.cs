using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TankWars
{
    /// <summary>
    /// A button that can be hovered over and clicked.
    /// </summary>
    class Button
    {
        /// <summary>
        /// The default sprite shown when the button is not hovered over.
        /// </summary>
        private Sprite m_sprite;

        /// <summary>
        /// The hover sprite shown when the button is hovered over.
        /// </summary>
        private Sprite m_hoverSprite;

        /// <summary>
        /// Triggered when the button is clicked.
        /// </summary>
        private event Action m_onClick;

        public Button(ContentManager content, string texture, string hoverTexture, Vector2 position)
        {
            m_sprite = new Sprite(content, texture, position);
            m_hoverSprite = new Sprite(content, hoverTexture, position);
        }

        public Button(ContentManager content, string texture, string hoverTexture, Vector2 position, Action onClick)
        {
            m_sprite = new Sprite(content, texture, position);
            m_hoverSprite = new Sprite(content, hoverTexture, position);
            m_onClick += onClick;
        }

        public Button(ContentManager content, string texture, Vector2 position)
            : this(content, texture, texture + "Hover", position)
        {
        }

        public Button(ContentManager content, string texture, Vector2 position, Action onClick)
            : this(content, texture, texture + "Hover", position, onClick)
        {
        }

        /// <summary>
        /// Returns true if the button is being hovered over.
        /// </summary>
        public bool IsMouseOver()
        {
            return m_sprite.Bounds.Contains(Mouse.GetState().Position);
        }

        /// <summary>
        /// Returns true if the button is clicked.
        /// </summary>
        public bool IsClicked(InputManager input)
        {
            return IsMouseOver() && input.IsJustReleased(MouseButtons.Left);
        }

        /// <summary>
        /// Called when the button should update.
        /// </summary>
        public void Update(InputManager input)
        {
            if (m_onClick != null && IsClicked(input))
            {
                m_onClick();
            }
        }

        /// <summary>
        /// Called when the button should draw.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsMouseOver())
            {
                m_hoverSprite.Draw(spriteBatch);
            }
            else
            {
                m_sprite.Draw(spriteBatch);
            }
        }
    }
}
