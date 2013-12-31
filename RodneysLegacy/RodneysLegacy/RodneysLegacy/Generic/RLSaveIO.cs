using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodneysLegacy
{
    //only needed to give convenient access to the creatures, could
    //just ret tilemap and be happy with searching for the player
    //creature myself, but that's annoying...
    class SavedLevel
    {
        public RLTileMap TileMap;
        public List<RLCreature> Creatures;
    }

    class RLSaveIO
    {
        public static SavedLevel LoadLevel(string _path)
        {
            //for now, any tilemap after the first found in file
            //is going to be ignored. several for one level might
            //be of use later, but right now we're not complicating
            //things unecessarily.

            //test file
            List<string> _file = new List<string>()
            {
               "#Creatures",
               "0;man;5,5",
               "#Tilemaps",
               "0;16,9",
               "#Tiles",
               "5,5;0;-1;1,1",
               "5,6;0;-1;1,0",
               "7,5;0;-1;0,1",
               "6,6;0;-1;1,0",
               "6,4;0;-1;0,1",
               "7,4;0;-1;0,1"
            };

            SavedLevel _sl = new SavedLevel();

            Dictionary<int, RLCreature> _creatures =
                new Dictionary<int, RLCreature>();
            Dictionary<int, RLTileMap> _tilemaps =
                new Dictionary<int, RLTileMap>();

            #region listmangling
            int _i = _file.IndexOf("#Tilemaps");
            int _j = _file.IndexOf("#Tiles");

            List<string> _creatureentries =
                _file
                .FindAll(x => true)
                //skip #Creatures header
                .Skip(1)
                .ToList();
            _creatureentries.RemoveRange(_i - 1, _file.Count - (_i));

            List<string> _tilemapentries = _file.FindAll(x => true);
            _tilemapentries.RemoveRange(_j, _file.Count - _j);
            _tilemapentries = _tilemapentries
                //skip last sections header and ours
                .Skip(_creatureentries.Count + 2)
                .ToList();

            List<string> _tileentries =
                _file.FindAll(x => true)
                //again, but with all earlier entries
                //and their and our headers
                .Skip(_creatureentries.Count + _tilemapentries.Count + 3)
                .ToList();
            #endregion

            //creatures
            foreach (string _entry in _creatureentries)
            {
            }

            //tilemaps
            RLTileMap _foo = LoadTileMap(_tilemapentries[0]);
            _tilemaps.Add(_foo.ID, _foo);
            if (_sl.TileMap == null) _sl.TileMap = _foo;

            //tiles
            foreach (string _entry in _tileentries)
            {
                LoadTile(
                    _entry,
                    ref _tilemaps,
                    ref _creatures
                );
            }

            return _sl;
        }

        public static RLTileMap LoadTileMap(string _tilemapstring)
        {
            List<string> _elements =
                _tilemapstring.Split(
                    new char[] { ';' } //wtf
            ).ToList();

            List<string> _size =
                _elements[1].Split(
                    new char[] { ',' }
            ).ToList();

            int _ID;
            int _width;
            int _height;

            Int32.TryParse(_elements[0], out _ID);
            Int32.TryParse(_size[0], out _width);
            Int32.TryParse(_size[1], out _height);

            RLTileMap _return = new RLTileMap(_width, _height);
            _return.ID = _ID;

            return _return;
        }

        public static RLTile LoadTile(
            string _tilestring,
            ref Dictionary<int, RLTileMap> _tilemaps,
            ref Dictionary<int, RLCreature> _creatures
        ) {
            RLTile _return;

            List<string> _elements =
                _tilestring.Split(
                    new char[] { ';' } //wtf
            ).ToList();

            List<string> _coord =
                _elements[0].Split(
                    new char[] { ',' }
            ).ToList();

            List<string> _walls =
                _elements[3].Split(
                    new char[] { ',' }
            ).ToList();

            int _x, _y;
            int _mapid; RLTileMap _map;
            int _creatureid; RLCreature _creature;
            int _north, _west;

            Int32.TryParse(_coord[0], out _x);
            Int32.TryParse(_coord[1], out _y);
            Int32.TryParse(_elements[1], out _mapid);
            _map = _tilemaps[_mapid];
            Int32.TryParse(_elements[2], out _creatureid);
            _creature = _creatureid == -1 ? null : _creatures[_creatureid];
            Int32.TryParse(_walls[0], out _north);
            Int32.TryParse(_walls[1], out _west);


            _return = new RLTile(_map, _x, _y);
            _return.Creature = _creature;
            _return.North = _north == 1;
            _return.West = _west == 1;

            _map[_x, _y] = _return; //add to tilemap

            return _return;
        }
    }
}
