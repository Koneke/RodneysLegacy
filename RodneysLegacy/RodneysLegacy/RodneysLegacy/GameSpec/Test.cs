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
                //source
            );

            game.World[5, 5].West = true;
            game.World[5, 5].North = true;
            game.World[5, 6].North = true;

            game.World[7, 5].West = true;
            game.World[6, 6].North = true;

            game.World[6, 4].West = true;
            game.World[7, 4].West = true;

            game.EventListeners.Add(
                new MoveEventListener()
            );
        }
    }
}
