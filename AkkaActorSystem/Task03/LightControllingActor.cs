using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;

namespace AkkaActorSystem.Task03
{
    public class LightControllingActor:ReceiveActor
    {
        private Dictionary<string, IActorRef> _dic;
        private int _counter;
        private ILoggingAdapter _log= Context.GetLogger();

        public LightControllingActor()
        {

            Receive<LightControlMessages.Start>(s =>
            {
                _dic = s.ActorRefs;
                _dic["P"].Tell(new LightControlMessages.PedestriansRed());
                _dic["NS"].Tell(new LightControlMessages.NorthSouthRed());
                _dic["EW"].Tell(new LightControlMessages.EastWestRed());
                _counter = 0;
            });

            Receive<LightControlMessages.Timer>(t =>
            {
                _dic["EW"].Tell(new LightControlMessages.EastWestGreen());
                Become(StateA);
            });

        }

        private void StateA() // EW green
        {
            Receive<LightControlMessages.PedestriansPushButtonActivated>(m => { _counter++; });
            Receive<LightControlMessages.Timer>(t =>
            {
                _dic["EW"].Tell(new LightControlMessages.EastWestRed());
                _dic["NS"].Tell(new LightControlMessages.NorthSouthGreen());
                Become(StateB);
            });
        }

        private void StateB() // NS green
        {
            Receive<LightControlMessages.PedestriansPushButtonActivated>(m => { _counter++; });
            Receive<LightControlMessages.Timer>(t =>
            {
                _dic["NS"].Tell(new LightControlMessages.NorthSouthRed());

                if (_counter > 0)
                {
                    _dic["EW"].Tell(new LightControlMessages.PedestriansGreen());
                    Become(StateC);
                }
                else
                {
                    _dic["EW"].Tell(new LightControlMessages.EastWestGreen());
                    Become(StateA);
                }
            });
        }

        private void StateC()
        {
            Receive<LightControlMessages.PedestriansPushButtonActivated>(
                e =>
                {
                    // ignore
                });

            Receive<LightControlMessages.Timer>(t =>
            {
                _counter = 0;
                _dic["EW"].Tell(new LightControlMessages.PedestriansRed());
                _dic["EW"].Tell(new LightControlMessages.EastWestGreen());
                Become(StateA);
            });
        }
    }
}
