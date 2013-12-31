using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodneysLegacy
{
    class RLTile
    {
        public RLTileMap Map;
        public int X;
        public int Y;

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
                    case 1: return Map[X - 1, Y + 1];
                    case 2: return Map[X, Y + 1];
                    case 3: return Map[X + 1, Y + 1];

                    case 4: return Map[X - 1, Y];
                    case 5: return this;
                    case 6: return Map[X + 1, Y];

                    case 7: return Map[X - 1, Y - 1];
                    case 8: return Map[X, Y - 1];
                    case 9: return Map[X + 1, Y - 1];

                    default: return null;
                }
            }
        }

        public bool Walkable(int _direction)
        {
            if (this[_direction] == null) return false;
            //don't overwrite other creatures....
            if (this[_direction].Creature != null) return false;

            switch (_direction)
            {
                case 1: return
                    (!this[1].North && !this[5].West) ||
                    (!this[2].North && !this[2].West);
                case 2: return !this[2].North;
                case 3: return
                    (!this[2].North && !this[3].West) ||
                    (!this[3].North && !this[6].West);

                case 4: return !this[5].West;
                case 6: return !this[6].West;

                case 7: return
                    (!this[5].North && !this[8].West) ||
                    (!this[4].North && !this[5].West);
                case 8: return !this[5].North;
                case 9: return
                    (!this[5].North && !this[9].West) ||
                    (!this[6].North && !this[6].West);

                default: return false;
            }
        }

        public RLTile(
            RLTileMap _map,
            int _x,
            int _y
        ) {
            Map = _map;
            X = _x;
            Y = _y;
        }

        public string CreateSaveString()
        {
            return
                X.ToString() + "," + Y.ToString() + ";" +
                Map.ID.ToString() + ";" +
                (North ? "1," : "0,") +
                (West ? "1" : "0");
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
