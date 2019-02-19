using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;

namespace AkkaActorSystem.Task03
{
    public class LightControllingActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> _dic;
        private int _counter;
        private ILoggingAdapter _log = Context.GetLogger();

        public LightControllingActor()
        {
            Receive<LightControlMessages.Start>(s =>
            {
                _log.Info($"In state:ctor Received  {nameof(LightControlMessages.Start)} message");
                _dic = s.ActorRefs;
                _dic["P"].Tell(new LightControlMessages.PedestriansRed());
                _dic["NS"].Tell(new LightControlMessages.NorthSouthRed());
                _dic["EW"].Tell(new LightControlMessages.EastWestRed());

                _counter = 0;
                _log.Info("counter set to 0");
            });

            Receive<LightControlMessages.Timer>(t =>
            {
                _log.Info($"In state:ctor Received {nameof(LightControlMessages.EastWestGreen)} message");
                _dic["EW"].Tell(new LightControlMessages.EastWestGreen());
                Become(StateA);
            });
        }

        private void StateA() // EW green
        {
            Receive<LightControlMessages.PedestriansPushButtonActivated>(m =>
            {
                _log.Info(
                    $"In state:{nameof(StateA)} Received {nameof(LightControlMessages.PedestriansPushButtonActivated)} message");
                _counter++;
            });

            Receive<LightControlMessages.Timer>(t =>
            {
                _log.Info($"In state:{nameof(StateA)} Received {nameof(LightControlMessages.Timer)} message");
                _dic["EW"].Tell(new LightControlMessages.EastWestRed());
                _dic["NS"].Tell(new LightControlMessages.NorthSouthGreen());
                _log.Info($"In state:{nameof(StateA)} switching to StateB ");
                Become(StateB);
            });
        }

        private void StateB() // NS green
        {
            Receive<LightControlMessages.PedestriansPushButtonActivated>(m =>
            {
                _log.Info(
                    $"In state:{nameof(StateB)} Received {nameof(LightControlMessages.PedestriansPushButtonActivated)} message");
                _counter++;
            });

            Receive<LightControlMessages.Timer>(t =>
            {
                _log.Info($"In state:{nameof(StateB)} Received {nameof(LightControlMessages.Timer)} message");
                _dic["NS"].Tell(new LightControlMessages.NorthSouthRed());
                _log.Info($"In state:{nameof(StateB)} counter: {_counter}");
                if (_counter > 0)
                {
                    _dic["EW"].Tell(new LightControlMessages.PedestriansGreen());
                    _log.Info($"In state:{nameof(StateB)} switching to {nameof(StateC)}");
                    Become(StateC);
                }
                else
                {
                    _dic["EW"].Tell(new LightControlMessages.EastWestGreen());
                    _log.Info($"In state:{nameof(StateB)} switching to {nameof(StateA)}");
                    Become(StateA);
                }
            });
        }

        private void StateC()
        {
            Receive<LightControlMessages.PedestriansPushButtonActivated>(
                e =>
                {
                    _log.Info(
                        $"In state:{nameof(StateC)} Received {nameof(LightControlMessages.Timer)} message - ignoring");
                    // ignore
                });

            Receive<LightControlMessages.Timer>(t =>
            {
                _counter = 0;
                _log.Info($"In state:{nameof(StateB)} counter set to: {_counter}");
                _dic["EW"].Tell(new LightControlMessages.PedestriansRed());
                _dic["EW"].Tell(new LightControlMessages.EastWestGreen());
                _log.Info($"In state:{nameof(StateC)} switching to {nameof(StateA)}");
                Become(StateA);
            });
        }
    }
}