using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace RodneysLegacy
{
    class PlayerInput
    {
        KeyboardState oks;
        RLGame game;
        //RLCreature player;

        public PlayerInput(
            RLGame _game
            //RLCreature _player
        ) {
            game = _game;
            //player = _player;
        }

        //returns whether or not anything was done
        public bool Handle()
        {
            bool _result = false;
            KeyboardState ks = Keyboard.GetState();

            for (
                //holy shiiiiiiit enums yo
                Keys _key = Keys.NumPad1;
                _key <= Keys.NumPad9;
                _key++
            ) {
                if (
                    ks.GetPressedKeys().ToList().Contains(_key) &&
                    !oks.GetPressedKeys().ToList().Contains(_key) &&
                    _key != Keys.NumPad5
                ) {
                    int _direction = (int)_key - ((int)Keys.NumPad1 - 1);

                    //if (player.Tile[_direction] != null)
                    if(game.Player.Tile.Walkable(_direction))
                    {
                        game.EventQueue.Add(
                            new MoveEvent(
                                game.Player,
                                null,
                                game.Player.Tile[_direction]
                            )
                        );
                        _result = true;
                    }
                }
            }

            oks = ks;
            return _result;
        }
    }
}
