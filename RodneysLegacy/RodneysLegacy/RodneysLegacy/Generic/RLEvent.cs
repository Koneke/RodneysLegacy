using System.Collections.Generic;

namespace RodneysLegacy
{
    //sample event process 1:
    //  player movement
    //key pressed, move event added to queue
    //  with info including moving creature (player),
    //  source tile, and destination tile.
    //moveListener checks for move events in the queue,
    //  and foreach of those, checks whether or not the
    //  move is legal. if it is, it does the actual move.

    interface IEvent
    {
    }

    interface IEventListener
    {
        void CheckQueue(
            List<IEvent> eventQueue,
            ref List<IEvent> newEvents
        );
    }
}
