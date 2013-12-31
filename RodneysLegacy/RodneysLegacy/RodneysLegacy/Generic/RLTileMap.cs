using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodneysLegacy
{
    class RLTileMap
    {
        RLTile[,] map;
        public int ID;
        public int Width;
        public int Height;
        public List<RLCreature> Creatures;

        public RLTileMap(int _w, int _h)
        {
            map = new RLTile[_w, _h];
            ID = -1;
            Width = _w;
            Height = _h;
            Creatures = new List<RLCreature>();

            //initialize
            for (int _x = 0; _x < _w; _x++)
                for (int _y = 0; _y < _h; _y++)
                    map[_x, _y] = new RLTile(this, _x, _y);
        }

        public RLTile this[int _x, int _y]
        {
            get
            {
                if (_x >= Width || _x < 0 ||
                    _y >= Height || _y < 0)
                    return null;

                return map[_x, _y];
            }
            set { map[_x, _y] = value; }
        }
    }
}
