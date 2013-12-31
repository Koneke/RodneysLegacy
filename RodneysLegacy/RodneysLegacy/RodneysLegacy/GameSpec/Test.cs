using System;
using System.Collections.Generic;

namespace RodneysLegacy
{
    class Test
    {
        public Test(RLGame game)
        {
            game.EventListeners.Add(
                new MoveEventListener()
            );

            SavedLevel _level = 
                RLSaveIO.LoadLevel(
                RLSaveIO.cwd+"/Content/Data/testsave");
            game.World = _level.TileMap;

            game.Player = _level.Creatures.Find(
                x => x.ID == 0);

            game.World.Creatures = _level.Creatures;
            foreach (RLCreature _c in game.World.Creatures)
            {
                _c.Brain = new RLBrain(_c, game);
            }

            game.Player.Brain = null; //heh...

            var a = RLTileMap.GetTilePath(
                game.World[5, 5],
                game.World[0, 0]
            );
            var b = 1;
        }
    }
}
