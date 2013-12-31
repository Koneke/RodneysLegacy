using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace RodneysLegacy
{
    class RLGame
    {
        public RLTileMap World;
        public List<IEvent> EventQueue;
        public List<IEventListener> EventListeners;
        public RenderTarget2D Render;
        public Random Random;
        public bool Quit;

        public RLCreature Player;
        PlayerInput playerInput;

        List<string> log;
        public void Log(string _s) { log.Add(_s); }
        public List<string> GetLog() { return log; }

        public RLGame()
        {
            World = new RLTileMap(16, 9);
            EventQueue = new List<IEvent>();
            EventListeners = new List<IEventListener>();

            Random = new Random();
            Quit = false;

            RLSaveIO.Setup();

            //Player = new RLCreature();
            playerInput = new PlayerInput(this);

            log = new List<string>();
            Log("foo");
            Log("bar");

            //dbg
            Test _test = new Test(this);
        }

        public void Input()
        {
            //if the player has done anything
            if (playerInput.Handle())
            {
                //run all brains here
                foreach(RLCreature _c in World.Creatures)
                    if(_c.Brain != null) _c.Brain.Think();
                process();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                RLSaveIO.SaveLevel(
                    RLSaveIO.cwd+"/Content/Data/testsave",
                    World, World.Creatures
                );
                Quit = true;
            }
        }

        void process()
        {
            //prepare new list for events triggered by the listeners
            //(like an ignite event triggering a fire damage event)
            List<IEvent> newEvents = new List<IEvent>();

            //send all the listeners the current queue,
            //letting them output any new events to the prepared list
            foreach (IEventListener _el in EventListeners)
                _el.CheckQueue(EventQueue, ref newEvents);

            //replace the old queue with the new stuff when we are done with it
            EventQueue.Clear();
            EventQueue.AddRange(newEvents);

            //if there were any new events triggered, keep processing
            if (EventQueue.Count > 0)
            {
                process();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //test fov stuff

            bool[,] _vismap =
                new bool[GameVars.ScreenWidth, GameVars.ScreenHeight];

            //Viswalker _vw = new Viswalker(new List<int>() { 7, 8, 9 } );
            Viswalker _vw = new Viswalker(Player.Facing);
            _vw.Tile = Player.Tile;
            _vw.Walk();
            var a = _vw.Visible();

            spriteBatch.Begin();
            for (int _x = 0; _x < World.Width; _x++)
            {
                for (int _y = 0; _y < World.Height; _y++)
                {
                    spriteBatch.Draw(
                        Res.Textures[World[_x, _y].Texture ?? "blank"],
                        new Rectangle(
                            _x * GameVars.TileSize,
                            _y * GameVars.TileSize,
                            GameVars.TileSize,
                            GameVars.TileSize
                        ),
                        a.Contains(World[_x, _y]) ? Color.White : Color.Blue
                    );

                    #region walls
                    bool _w = World[_x, _y].West;
                    bool _n = World[_x, _y].North;

                    Vector2 _wallSource = new Vector2(
                        _n ? GameVars.TileSize : (_w ? 1 : 0),
                        _w ? GameVars.TileSize : (_n ? 1 : 0)
                    );

                    //looks hackish, but is to avoid the missing pixel
                    //on the 8w/4n corner if x,y does not have walls
                    if (!(_w || _n) && _x > 0 && _y > 0)
                    {
                        if (World[_x, _y][8].West &&
                            World[_x, _y][4].North)
                        {
                            _wallSource.X = 1;
                            _wallSource.Y = 1;
                        }
                    }

                    spriteBatch.Draw(
                        Res.Textures["grid"],
                        new Rectangle(
                            _x * GameVars.TileSize,
                            _y * GameVars.TileSize,
                            //_n ? GameVars.TileSize : (_w ? 1 : 0),
                            //_w ? GameVars.TileSize : (_n ? 1 : 0)
                            (int)_wallSource.X,
                            (int)_wallSource.Y
                        ),
                        new Rectangle(
                            0,
                            0,
                            //_n ? GameVars.TileSize : (_w ? 1 : 0),
                            //_w ? GameVars.TileSize : (_n ? 1 : 0)
                            (int)_wallSource.X,
                            (int)_wallSource.Y
                        ),
                        Color.Black
                    );
                    #endregion

                    if (World[_x, _y].Creature != null)
                    {
                        spriteBatch.Draw(
                            Res.Textures[World[_x, _y].Creature.Texture],
                            new Rectangle(
                                _x * GameVars.TileSize,
                                _y * GameVars.TileSize,
                                GameVars.TileSize,
                                GameVars.TileSize
                            ),
                            Color.White
                        );
                    }
                }
            }

            spriteBatch.End();
        }

        public void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            #region drawlog

            string _string = log.Count > 0 ? log[log.Count - 1] : "";
            _string = Utility.WrapText(
                Res.Fonts["logfont"], _string, GameVars.WindowWidth);
            Vector2 _position;

            float _currentY = 0;

            for (int _i = 0; _i < 3; _i++)
            {
                _string = log.Count - _i > 0 ? log[(log.Count - 1) - _i] : "";
                if(_string == "") continue;

                _string = Utility.WrapText(
                    Res.Fonts["logfont"], _string, GameVars.WindowWidth);

                _currentY += Res.Fonts["logfont"]
                    .MeasureString(_string).Y + 2;

                _position =
                    new Vector2(
                        6,
                        GameVars.WindowHeight - _currentY
                    );

                DrawTextBackground(
                    spriteBatch,
                    _position,
                    _string
                );

                spriteBatch.DrawString(
                    Res.Fonts["logfont"],
                    _string,
                    _position,
                    Color.White
                );
            }
            #endregion
            #region drawstatline
            string _statline =
                "stat: 1 | foo: bar | hungry?: mhm\nhi i'm a newline";
            _position =
                new Vector2(6, 2);
            DrawTextBackground(
                spriteBatch,
                _position,
                _statline
            );
            spriteBatch.DrawString(
                Res.Fonts["logfont"],
                _statline,
                _position,
                Color.White
            );
            #endregion
            spriteBatch.End();
        }

        public void DrawTextBackground(
            SpriteBatch spriteBatch,
            Vector2 _position,
            string _text
        ) {
            Vector2 _measure = Res.Fonts["logfont"]
                .MeasureString(_text);

            spriteBatch.Draw(
                Res.Textures["blank"],
                new Rectangle(
                    (int)_position.X-2,
                    (int)_position.Y,
                    (int)_measure.X+4,
                    (int)_measure.Y
                ),
                Color.Black
            );
        }
    }
}
