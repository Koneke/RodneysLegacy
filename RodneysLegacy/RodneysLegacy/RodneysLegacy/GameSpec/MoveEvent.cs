using System.Collections.Generic;

namespace RodneysLegacy
{
    class MoveEvent : IEvent
    {
        public RLCreature Actor;
        public RLTile Source;
        public RLTile Destination;

        public MoveEvent(
            RLCreature _actor,
            RLTile _source,
            RLTile _destination
        ) {
            Actor = _actor;
            Source = _source ?? _actor.Tile;
            Destination = _destination;
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
                    _me.Destination);
            }
        }
    }
}
