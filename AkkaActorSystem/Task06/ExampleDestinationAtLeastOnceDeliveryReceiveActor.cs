using Akka.Actor;
using Akka.Event;

namespace AkkaActorSystem.Task06
{
    public class ExampleDestinationAtLeastOnceDeliveryReceiveActor : ReceiveActor
    {
        private bool _shallIRespond;
        private ILoggingAdapter _log = Context.GetLogger();

        public ExampleDestinationAtLeastOnceDeliveryReceiveActor()
        {
            Receive<PersistentMessages.WithDeliveryId>(msg =>
            {
                _shallIRespond = !_shallIRespond;
                _log.Info($"Received message, shall I respond: {_shallIRespond}");

                if (_shallIRespond)
                {
                    Sender.Tell(new PersistentMessages.Confirm(msg.DeliveryId, msg.MessageSentSender));
                }
            });
        }
    }
}