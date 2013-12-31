using System.Collections.Generic;

namespace RodneysLegacy
{
    class MoveEvent : IEvent
    {
        public RLCreature Actor;
        public RLTile Source;
        public int Direction;

        public MoveEvent(
            RLCreature _actor,
            int _direction,
            RLTile _source = null
        ) {
            Actor = _actor;
            Source = _source ?? _actor.Tile;
            Direction = _direction;
        }
    }

    class MoveEventListener : IEventListener
    {
        public void CheckQueue(
            List<IEvent> eventQueue,
            ref List<IEvent> newEvents
        ) {
            foreach (
                IEvent _e in eventQueue
                    .FindAll(x => x is MoveEvent)
            ) {
                MoveEvent _me = _e as MoveEvent;
                RLTile.Move(
                    _me.Actor,
                    _me.Source,
                    _me.Source[_me.Direction]
                );
                _me.Actor.Facing = _me.Direction;
            }
        }
    }
}
