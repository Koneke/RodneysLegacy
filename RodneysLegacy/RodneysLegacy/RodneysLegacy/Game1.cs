using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RodneysLegacy
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RLGame Game;

        public Game1()
        {
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Game = new RLGame();
            Game.Render = new RenderTarget2D(
                graphics.GraphicsDevice,
                GameVars.TileSize * GameVars.ScreenWidth,
                GameVars.TileSize * GameVars.ScreenHeight
            );
            GameVars.WindowWidth = graphics.PreferredBackBufferWidth;
            GameVars.WindowHeight = graphics.PreferredBackBufferHeight;
            Res.Textures = new Dictionary<string, Texture2D>();
            Res.Fonts = new Dictionary<string, SpriteFont>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Res.Textures.Add("man", Content.Load<Texture2D>("man"));
            Res.Textures.Add("grid", Content.Load<Texture2D>("grid"));
            Res.Textures.Add("blank", Content.Load<Texture2D>("blank"));

            Res.Fonts.Add("logfont", Content.Load<SpriteFont>("logfont"));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            Game.Input();
            if (Game.Quit) this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //render the game
            GraphicsDevice.SetRenderTarget(Game.Render);
            Game.Draw(spriteBatch);
            GraphicsDevice.SetRenderTarget(null);

            //draw without blurring stuff
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null
            );
            spriteBatch.Draw(
                Game.Render,
                GraphicsDevice.PresentationParameters.Bounds,
                Color.White
            );
            spriteBatch.End();

            Game.DrawUI(spriteBatch);
            
            //idk do xna stuff
            base.Draw(gameTime);
        }
    }
}
