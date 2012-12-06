using KJFramework.Game.Basic.Enums;
using KJFramework.Game.Components.Controls;
using KJFramework.Game.Components.Controls.Input;
using KJFramework.Game.Components.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ComponentTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D _texture2D;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.ToggleFullScreen();
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 700;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _texture2D = Content.Load<Texture2D>("hat079");
            // TODO: Add your initialization logic here
            //MousePointer mousePointer = new MousePointer(this);
            //mousePointer.Images.Add(MouseStateTypes.Normal, Content.Load<Texture2D>("point"));
            Button button = new Button(this);
            Texture2D normal = Content.Load<Texture2D>("001276F900000000");
            button.Images.Add(ButtonStateTypes.Normal, normal);
            button.Images.Add(ButtonStateTypes.MouseEnter, Content.Load<Texture2D>("0012740A00000000"));
            button.Images.Add(ButtonStateTypes.Click, Content.Load<Texture2D>("0012740A00000000"));
            button.Width = normal.Width;
            button.Height = normal.Height;
            Window window = new Window(this);
            window.CanMove = true;
            window.UseBackgroundSetBound = true;
            window.Background = Content.Load<Texture2D>("001EC3CA00000000");
            window.Position = new Vector2(100, 100);
            float currentHeight = 60;
            float currentWidth = 88;
            for (int i = 0; i < 11; i++)
            {
                Label label = new Label(this);
                label.Caption = "1000" + i;
                label.Font = Content.Load<SpriteFont>("de");
                label.FontColor = Color.White;
                label.Initialize();
                if(i == 0)
                {
                    window.RegistControl("Label" + i, new WindowControlPair { Control = label, Position = new Vector2(currentWidth, currentHeight) });
                }
                else if(i < 11)
                {
                    currentHeight += 26;
                    window.RegistControl("Label" + i, new WindowControlPair { Control = label, Position = new Vector2(currentWidth, currentHeight) });
                }
            }
            window.RegistControl("CloseButton", new WindowControlPair { Control = button, Position = new Vector2(window.Width - normal.Width - 15, 11)});
            Components.Add(window);
            //Components.Add(mousePointer);
            Components.Add(new GamerServicesComponent(this));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Pink);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            spriteBatch.Draw(_texture2D, new Rectangle(0, 0, _texture2D.Width, _texture2D.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
