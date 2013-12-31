using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodneysLegacy
{
    class RLTile
    {
        RLTileMap map;
        int x;
        int y;

        public RLCreature Creature;
        public string Texture;

        public bool West;
        public bool North;

        //get surrounding tile by keypad direction
        public RLTile this[int _direction]
        {
            get
            {
                switch (_direction)
                {
                    case 1: return map[x - 1, y + 1];
                    case 2: return map[x, y + 1];
                    case 3: return map[x + 1, y + 1];

                    case 4: return map[x - 1, y];
                    case 6: return map[x + 1, y];

                    case 7: return map[x - 1, y - 1];
                    case 8: return map[x, y - 1];
                    case 9: return map[x + 1, y - 1];

                    default: return null;
                }
            }
        }

        public bool Walkable(int _direction)
        {
            RLTile _target = this[_direction];
            if (_target == null) return false;

            switch (_direction)
            {
                case 1: return (!_target.North || !West);
                case 2: return !_target.North;
                case 3: return (!_target.North || !_target.West);

                case 4: return !West;
                case 6: return !_target.West;

                case 7: return (!North || !West);
                case 8: return !North;
                case 9: return (!North || !_target.West);

                default: return false;
            }
        }

        public RLTile(
            RLTileMap _map,
            int _x,
            int _y
        ) {
            map = _map;
            x = _x;
            y = _y;
        }

        public static void Move(
            RLCreature _actor,
            RLTile _source,
            RLTile _destination
        ) {
            if(_source != null)
                _source.Creature = null;
            _destination.Creature = _actor;
            _actor.Tile = _destination;
        }
    }
}
