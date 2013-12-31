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
        public static List<string> SaveLevel(
            string _path,
            RLTileMap _tileMap,
            List<RLCreature> _creatures
        ) {
            List<string> _level = new List<string>();

            _level.Add("#Tilemaps");
            _level.Add(_tileMap.CreateSaveString());

            _level.Add("#Tiles");
            for (int _x = 0; _x < _tileMap.Width; _x++)
            {
                for (int _y = 0; _y < _tileMap.Width; _y++)
                {
                    if (_tileMap[_x, _y] != null)
                        _level.Add(_tileMap[_x, _y].CreateSaveString());
                }
            }

            _level.Add("#Creatures");
            foreach (RLCreature _c in _creatures)
                _level.Add(_c.CreateSaveString());

            return _level;
        }

        public static SavedLevel LoadLevel(string _path)
        {
            //for now, any tilemap after the first found in file
            //is going to be ignored. several for one level might
            //be of use later, but right now we're not complicating
            //things unecessarily.

            //test file
            List<string> _file = new List<string>()
            {
               "#Tilemaps",
               "0;16,9",
               "#Tiles",
               "5,5;0;1,1",
               "5,6;0;1,0",
               "7,5;0;0,1",
               "6,6;0;1,0",
               "6,4;0;0,1",
               "7,4;0;0,1",
               "#Creatures",
               "0;0;man;5,5",
               "1;0;man;9,5"
            };

            SavedLevel _sl = new SavedLevel();
            List<RLCreature> _levelCreatures = new List<RLCreature>();

            Dictionary<int, RLCreature> _creatures =
                new Dictionary<int, RLCreature>();
            Dictionary<int, RLTileMap> _tilemaps =
                new Dictionary<int, RLTileMap>();

            #region listmangling
            int _i = _file.IndexOf("#Tiles");
            int _j = _file.IndexOf("#Creatures");

            List<string> _tileMapEntries =
                _file
                .FindAll(x => true)
                .Skip(1)
                .ToList();
            _tileMapEntries.RemoveRange(_i - 1, _file.Count - (_i));

            List<string> _tileEntries = _file.FindAll(x => true);
            _tileEntries.RemoveRange(_j, _file.Count - _j);
            _tileEntries = _tileEntries
                .Skip(_tileMapEntries.Count + 2)
                .ToList();

            List<string> _creatureEntries =
                _file.FindAll(x => true)
                .Skip(_tileMapEntries.Count + _tileEntries.Count + 3)
                .ToList();
            #endregion

            //tilemaps
            RLTileMap _map = LoadTileMap(_tileMapEntries[0]);
            _tilemaps.Add(_map.ID, _map);

            if (_sl.TileMap == null) _sl.TileMap = _map;

            //tiles
            foreach (string _entry in _tileEntries)
            {
                LoadTile(
                    _entry,
                    ref _tilemaps
                );
            }

            //creatures
            foreach (string _entry in _creatureEntries)
            {
                _levelCreatures.Add(
                    LoadCreature(
                        _entry,
                        ref _tilemaps
                    )
                );
            }

            _sl.Creatures = _levelCreatures;

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
            ref Dictionary<int, RLTileMap> _tilemaps
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
                _elements[2].Split(
                    new char[] { ',' }
            ).ToList();

            int _x, _y;
            int _mapid; RLTileMap _map;
            //int _creatureid; RLCreature _creature;
            int _north, _west;

            Int32.TryParse(_coord[0], out _x);
            Int32.TryParse(_coord[1], out _y);
            Int32.TryParse(_elements[1], out _mapid);
            _map = _tilemaps[_mapid];
            //Int32.TryParse(_elements[2], out _creatureid);
            //_creature = _creatureid == -1 ? null : _creatures[_creatureid];
            Int32.TryParse(_walls[0], out _north);
            Int32.TryParse(_walls[1], out _west);


            _return = new RLTile(_map, _x, _y);
            //_return.Creature = _creature;
            _return.Creature = null; //setting this in creature loading instead
            _return.North = _north == 1;
            _return.West = _west == 1;

            _map[_x, _y] = _return; //add to tilemap

            return _return;
        }

        public static RLCreature LoadCreature(
            string _entry,
            ref Dictionary<int, RLTileMap> _tilemaps
        ) {
            RLCreature _return;

            List<string> _elements =
                _entry.Split(
                    new char[] { ';' } //wtf
            ).ToList();

            List<string> _coord =
                _elements[3].Split(
                    new char[] { ',' }
            ).ToList();

            int _ID;
            int _mapID;
            int _x, _y;

            Int32.TryParse(_elements[0], out _ID);
            Int32.TryParse(_elements[1], out _mapID);
            Int32.TryParse(_coord[0], out _x);
            Int32.TryParse(_coord[1], out _y);

            _return = new RLCreature();
            _return.ID = _ID;
            _return.Texture = _elements[2];
            _return.Tile = _tilemaps[_mapID][_x, _y];
            _return.Tile.Creature = _return;

            return _return;
        }
    }
}
