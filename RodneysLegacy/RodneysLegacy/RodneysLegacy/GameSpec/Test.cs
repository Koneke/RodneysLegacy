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
                _c.Brain = new ChaserBrain(_c, game);
            }

            game.Player.Brain = null; //heh...
        }
    }

    class Viswalker
    {
        public List<int> Directions;
        public RLTile Tile;
        public List<Viswalker> Children;

        public Viswalker(int _direction)
        {
            //get directions from facing, so add neighbours
            Directions = new List<int>();
            Directions.Add(_direction);
            for (int _i = 1; _i <= 9; _i++)
            {
                if (Utility.KeypadNeighbour(_i, _direction))
                {
                    Directions.Add(_i);
                }
            }
        }

        public Viswalker(List<int> _directions)
        {
            Directions = _directions.FindAll(x=>true);
        }

        public void Walk()
        {
            if (Children == null)
            {
                Children = new List<Viswalker>();
                List<int> nonBlocked = Directions.FindAll(x => true);
                foreach (int _direction in Directions)
                {
                    if (!Tile.Walkable(_direction))
                        nonBlocked.Remove(_direction);
                }
                foreach (int _direction in nonBlocked)
                {
                    Viswalker _child = new Viswalker(nonBlocked);
                    _child.Tile = Tile[_direction];
                    foreach (int _d in _child.Directions.FindAll(x => true))
                    {
                        if (!Utility.KeypadNeighbour(
                            _d, _direction)
                            && _d != _direction)
                            _child.Directions.Remove(_d);
                    }
                    Children.Add(_child);
                }
            }
            foreach (Viswalker _c in Children)
            {
                _c.Walk();
            }
        }

        public List<RLTile> Visible()
        {
            List<RLTile> _foo = new List<RLTile>();
            _foo.Add(Tile);
            foreach (Viswalker _c in Children)
            {
                _foo.AddRange(_c.Visible());
            }
            return _foo;
        }
    }
}
