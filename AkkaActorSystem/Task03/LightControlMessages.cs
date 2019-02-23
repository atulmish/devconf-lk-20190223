using System.Collections.Generic;
using Akka.Actor;

namespace AkkaActorSystem.Task03
{
    public class LightControlMessages
    {
        public class Timer
        {
        }

        public class NorthSouthRed
        {
        }

        public class NorthSouthGreen
        {
        }

        public class EastWestGreen
        {
        }

        public class EastWestRed
        {
        }

        public class PedestriansRed
        {
        }

        public class PedestriansGreen
        {
        }

        public class PedestriansPushButtonActivated
        {
        }

        public class Start
        {
            public Dictionary<string, IActorRef> ActorRefs { get; }

            public Start(Dictionary<string, IActorRef> actorRefs)
            {
                ActorRefs = actorRefs;
            }
        }
    }
}