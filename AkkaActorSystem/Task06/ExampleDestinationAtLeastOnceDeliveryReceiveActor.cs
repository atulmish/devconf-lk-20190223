using Akka.Actor;

namespace AkkaActorSystem.Task06
{
    public class ExampleDestinationAtLeastOnceDeliveryReceiveActor : ReceiveActor
    {
        public ExampleDestinationAtLeastOnceDeliveryReceiveActor()
        {
            Receive<PersistentMessages.WithDeliveryId>(msg =>
            {
                Sender.Tell(new PersistentMessages.Confirm(msg.DeliveryId), Self);
            });
        }
    }
}