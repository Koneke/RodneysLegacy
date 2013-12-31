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

        public string CreateSaveString()
        {
            return
                ID.ToString() + ";" +
                Width.ToString() + "," + Height.ToString();
        }

        public static List<RLTile> GetTilePath(
            RLTile _start,
            RLTile _destination)
        {
            List<RLTile> _return = new List<RLTile>();
            List<PathNode> _path = GetPath(_start, _destination);
            foreach (PathNode _pn in _path)
            {
                _return.Add(_pn.Tile);
            }
            return _return;
        }

        static List<PathNode> GetPath(
            RLTile _start,
            RLTile _destination)
        {
            List<PathNode> _path = new List<PathNode>();

            PathNode _startNode = new PathNode(null);
            _startNode.Tile = _start;
            List<PathNode> _open = new List<PathNode>() { _startNode };
            List<PathNode> _closed = new List<PathNode>();

            PathNode _current = _open[0];

            Func<PathNode, float> birdScore =
                _t =>
                    (float)(
                        Math.Sqrt(
                            Math.Pow(Math.Abs(_t.Tile.X - _destination.X), 2) +
                            Math.Pow(Math.Abs(_t.Tile.Y - _destination.Y), 2)
                        )
                    );

            bool _pathFound = false;

            while (_pathFound == false)
            {
                //add all good neighbours
                for (int _i = 1; _i <= 9; _i++)
                {
                    if (_i == 5) continue;
                    if (_current.Tile[_i] == _destination)
                    {
                        _pathFound = true;
                        while (_current.Parent != null)
                        {
                            _path.Add(_current);
                            _current = _current.Parent;
                        }
                        List<PathNode> _reversePath = new List<PathNode>();
                        /*for (int _n = _path.Count-1; _n >= 0; _n--)
                        {
                            _reversePath.Add(
                                new PathNode(
                                    _path[_n],
                                    10 - _path[_n]
                                        .ParentDirection
                                )
                            );
                        }
                        _reversePath.Add(_current);*/
                        for (int _n = 0; _n < _path.Count; _n++)
                        {
                            _reversePath.Insert(0, _path[_n]);
                            _reversePath[0].ParentDirection =
                                10 - _reversePath[0].ParentDirection;
                        }
                        return _reversePath;
                    }
                    if (
                        //walkable and not already in _open
                        _current.Tile.Walkable(_i) &&
                        !_open.Any(
                            _t => _t.Tile == _current.Tile[_i])
                        )
                    {
                        //create a new node, cleverly pointing back to
                        //the _current node
                        _open.Add(
                            new PathNode(
                                //_current.Tile[_i],
                                _current,
                                _current.PathScore +
                                    ((10 - _i) % 2 == 0 ? 10 : 14),
                                10 - _i
                            )
                        );
                    }
                }
                _open.Remove(_current);
                _closed.Add(_current);

                //no path was found
                if (_open.Count == 0)
                    return null;

                //grab the tile with the best score
                _current = _open.OrderBy(
                    _t => _t.PathScore + birdScore(_t)
                ).ToList()[0];
            }

            throw new Exception("PEBKAC");
        }
    }

    class PathNode
    {
        public PathNode Parent;
        public RLTile Tile;
        public int PathScore; //G
        public int? ParentDirection;

        public PathNode(
            PathNode _parent
            )
            : this(_parent, 0, null) { }

        public PathNode(
            PathNode _parent,
            int? _direction
            )
            : this(_parent, 0, _direction) { }

        public PathNode(
            PathNode _parent,
            int _score,
            int? _direction)
        {
            Parent = _parent;
            if(Parent != null)
                Tile = _parent.Tile[10-_direction.Value];
            PathScore = _score;
            ParentDirection = _direction;
        }
    }
}
