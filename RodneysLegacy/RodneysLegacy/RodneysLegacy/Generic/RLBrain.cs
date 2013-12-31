using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodneysLegacy
{
    class RLBrain
    {
        protected RLCreature myDude;
        protected RLGame game;

        public RLBrain(RLCreature _myDude) : this(_myDude, null) { }
        public RLBrain(
            RLCreature _myDude,
            RLGame _game
        ) {
            myDude = _myDude;
            game = _game;
        }

        //default, dumb think
        public virtual void Think()
        {
            List<int> _directions = new List<int>()
                { 1, 2, 3, 4, 6, 7, 8, 9 };
            _directions = _directions.FindAll(
                x => myDude.Tile.Walkable(x)
            );
            int _direction = _directions[
                game.Random.Next(0, _directions.Count)];
            game.EventQueue.Add(
                new MoveEvent(
                    myDude,
                    _direction
                )
            );
        }
    }
}
