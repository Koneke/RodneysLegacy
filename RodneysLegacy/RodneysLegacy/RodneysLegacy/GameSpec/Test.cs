using System;
using System.Collections.Generic;

namespace RodneysLegacy
{
    class Test
    {
        public Test(RLGame game)
        {
            game.Player.Texture = "man";
            RLTile.Move(
                game.Player,
                null, //from nowhere since it's a spawn
                game.World[5, 5]
            );

            game.EventListeners.Add(
                new MoveEventListener()
            );

            SavedLevel _level = 
                RLSaveIO.LoadLevel("foo");
            game.World = _level.TileMap;
        }
    }
}
