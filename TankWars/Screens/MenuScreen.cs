using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWars
{
    /// <summary>
    /// The main menu screen, where players can choose to play/edit a level or show the help.
    /// </summary>
    sealed class MenuScreen : BaseScreen
    {
        // The background texture and coin animation.
        private Texture2D m_background;
        private Animation m_coinAnim;

        // The tank 3D model and its current angle of rotation.
        private Model m_tankModel;
        private float m_tankAngle;

        // The menu buttons.
        private Button m_startButton;
        private Button m_editorButton;
        private Button m_helpButton;
        private Button m_exitButton;

        // Has the screen been covered by another screen.
        private bool m_visible;

        // Draws the background, even if the screen is not visible.
        private bool m_backgroundVisible;
        
        public MenuScreen(GraphicsDevice graphicsDevice, ContentManager content, ScreenManager screens, InputManager input)
            : base(graphicsDevice, content, screens, input)
        {
            // Load the background texture and coin animation.
            m_background = Content.Load<Texture2D>("Menu/Background");
            m_coinAnim = new Animation(Content.Load<Texture2D>("Menu/CoinAnim"), (1 / 8f), 8, 1);

            // Load the tank 3D model and set its initial angle of rotation.
            m_tankModel = Content.Load<Model>("Splash/Tank");
            m_tankAngle = 0;

            // Create and load the menu buttons.
            m_startButton = new Button(Content, "Menu/Buttons/Start", new Vector2(300, 600));
            m_editorButton = new Button(Content, "Menu/Buttons/Editor", new Vector2(500, 600));

            m_helpButton = new Button(Content, "Menu/Buttons/Help", new Vector2(700, 600), () => {
                // Load the help screen.
                Screens.Push(new HelpScreen(this));
            });

            m_exitButton = new Button(Content, "Menu/Buttons/Exit", new Vector2(900, 600), () => {
                // If all screens are removed, the game will exit.
                Screens.Clear();
            });

            // Set screen to be visible.
            m_visible = true;
            m_backgroundVisible = true;
        }

        /// <summary>
        /// Called when the screen should update.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (m_backgroundVisible)
            {
                // Update the coin animation and tank angle of rotation.
                m_coinAnim.Update(gameTime);
                m_tankAngle += 0.01f;
            }

            if (m_visible)
            {
                m_startButton.Update(Input);
                m_editorButton.Update(Input);
                m_helpButton.Update(Input);
                m_exitButton.Update(Input);

                if (m_startButton.IsClicked(Input))
                {
                    // Load the level select screen. When a level is selected, load the game screen with the selected level.
                    Screens.Push(new SelectScreen(this, selectedLevel => {
                        Screens.Push(new GameScreen(this, selectedLevel));
                        m_backgroundVisible = false;
                        m_visible = false;
                    }));
                }

                if (m_editorButton.IsClicked(Input))
                {
                    // Load the level select screen. When a level is selected, load the level editor screen with the selected level.
                    Screens.Push(new SelectScreen(this, level => {
                        Screens.Push(new EditorScreen(this, level));
                        m_backgroundVisible = false;
                        m_visible = false;
                    }));
                }
            }
        }

        /// <summary>
        /// Called when the screen should draw.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_backgroundVisible)
            {
                spriteBatch.Begin();

                // Draw the background texture and coin animation.
                spriteBatch.Draw(m_background, Vector2.Zero, Color.White);
                m_coinAnim.Draw(spriteBatch, new Vector2(300, 80));
                m_coinAnim.Draw(spriteBatch, new Vector2(975, 80), SpriteEffects.FlipHorizontally);

                spriteBatch.End();

                GraphicsDevice.DepthStencilState = new DepthStencilState { DepthBufferEnable = true };

                // Draw the tank 3D model.
                var world = Matrix.CreateRotationY(m_tankAngle);
                var view = Matrix.CreateLookAt(new Vector3(5, 3, 5), new Vector3(0, 0.5f, 0), Vector3.UnitY);
                var proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 1000);
                DrawModel(m_tankModel, world, view, proj);

                GraphicsDevice.DepthStencilState = new DepthStencilState { DepthBufferEnable = false };
            }

            if (m_visible)
            {
                spriteBatch.Begin();

                // Draw the menu buttons.
                m_startButton.Draw(spriteBatch);
                m_editorButton.Draw(spriteBatch);
                m_helpButton.Draw(spriteBatch);
                m_exitButton.Draw(spriteBatch);

                spriteBatch.End();
            }
        }

        /// <summary>
        /// Called when the screen is covered by another screen.
        /// </summary>
        public override void Covered(IScreen other)
        {
            m_visible = false;
            m_backgroundVisible = false;

            // If the select screen or help screen is covering this one, draw only the background.
            if (other is SelectScreen || other is HelpScreen)
            {
                m_backgroundVisible = true;
            }
        }

        /// <summary>
        /// Called when the screen is uncovered (after another screen is removed).
        /// </summary>
        public override void Uncovered(IScreen other)
        {
            m_visible = true;
            m_backgroundVisible = true;

            // If anything other than the select screen or help screen is uncovering this one, draw only the background.
            if (!(other is SelectScreen || other is HelpScreen))
            {
                m_visible = false;
            }
        }

        /// <summary>
        /// Draws a 3D model.
        /// </summary>
        private void DrawModel(Model model, Matrix world, Matrix view, Matrix proj)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = proj;
                }

                mesh.Draw();
            }
        }
    }
}
