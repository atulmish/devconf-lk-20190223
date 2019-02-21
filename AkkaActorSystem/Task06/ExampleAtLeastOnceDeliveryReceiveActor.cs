using Akka.Actor;
using Akka.Persistence;

namespace AkkaActorSystem.Task06
{
    public class ExampleAtLeastOnceDeliveryReceiveActor : AtLeastOnceDeliveryReceiveActor
    {
        private readonly IActorRef _destionationActor = Context.ActorOf<ExampleDestinationAtLeastOnceDeliveryReceiveActor>();

        public ExampleAtLeastOnceDeliveryReceiveActor()
        {
            Recover<PersistentMessages.MessageSent>(msgSent => Handler(msgSent));
            Recover<PersistentMessages.MsgConfirmed>(msgConfirmed => Handler(msgConfirmed));

            Command<string>(str =>
            {
                Persist(new PersistentMessages.MessageSent(str), Handler);
            });

            Command<PersistentMessages.Confirm>(confirm =>
            {
                Persist(new PersistentMessages.MsgConfirmed(confirm.DeliveryId), Handler);
            });
        }

        private void Handler(PersistentMessages.MessageSent messageSent)
        {
            Deliver(_destionationActor.Path, l => new PersistentMessages.WithDeliveryId(l, messageSent.Message));
        }

        private void Handler(PersistentMessages.MsgConfirmed msgConfirmed)
        {
            ConfirmDelivery(msgConfirmed.DeliveryId);
        }

        public override string PersistenceId { get; } = "persistence-id";
    }
}