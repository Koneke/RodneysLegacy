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
            List<PathNode> _path = RLTileMap.GetPath(
                myDude.Tile,
                game.Player.Tile);
            if (_path.Count > 0)
            {
                game.EventQueue.Add(
                    new MoveEvent(
                        myDude,
                        _path[0].ParentDirection.Value
                    )
                );
            }
        }
    }
}
