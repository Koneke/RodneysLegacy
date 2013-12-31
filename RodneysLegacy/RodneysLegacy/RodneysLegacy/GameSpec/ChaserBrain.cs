using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodneysLegacy
{
    class ChaserBrain : RLBrain
    {
        public ChaserBrain(
            RLCreature _myDude,
            RLGame _game)
            : base(_myDude, _game) { }

        public override void Think()
        {
            List<RLTile> _path = RLTileMap.GetTilePath(
                myDude.Tile,
                game.Player.Tile);
            if (_path.Count > 0)
            {
                game.EventQueue.Add(
                    new MoveEvent(
                        myDude,
                        null,
                        _path[0]
                    )
                );
            }
        }
    }
}
