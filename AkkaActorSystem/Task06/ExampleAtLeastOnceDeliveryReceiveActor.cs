using Akka.Actor;
using Akka.Event;
using Akka.Persistence;

namespace AkkaActorSystem.Task06
{
    public class ExampleAtLeastOnceDeliveryReceiveActor : AtLeastOnceDeliveryReceiveActor
    {
        private ILoggingAdapter _log = Context.GetLogger();

        private readonly IActorRef _destionationActor = Context.ActorOf<ExampleDestinationAtLeastOnceDeliveryReceiveActor>();
        private int _received = 1;

        public ExampleAtLeastOnceDeliveryReceiveActor()
        {
            Recover<PersistentMessages.MessageSent>(msgSent => Handler(msgSent));
            Recover<PersistentMessages.MsgConfirmed>(msgConfirmed => Handler(msgConfirmed));

            Command<string>(str =>
            {
                Persist(new PersistentMessages.MessageSent(str, Sender), Handler);
            });

            Command<PersistentMessages.Confirm>(confirm =>
            {
                Persist(new PersistentMessages.MsgConfirmed(confirm.DeliveryId,confirm.OrginalSender), Handler);
            });
        }

        private void Handler(PersistentMessages.MessageSent messageSent)
        {
            Deliver(_destionationActor.Path, l => new PersistentMessages.WithDeliveryId(l, messageSent.Message, messageSent.Sender));
        }

        private void Handler(PersistentMessages.MsgConfirmed msgConfirmed)
        {
            ConfirmDelivery(msgConfirmed.DeliveryId);
            _log.Info($"confirming message with id: {msgConfirmed.DeliveryId}, msg #{_received++}");
            msgConfirmed.OrginalSender.Tell("All is ok");
        }

        public override string PersistenceId { get; } = "persistence-id";
    }
}
