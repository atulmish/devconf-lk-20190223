using Akka.Actor;

namespace AkkaActorSystem.Task06
{
    public class PersistentMessages
    {
        public class WithDeliveryId
        {
            public WithDeliveryId(long deliveryId, string message, IActorRef messageSentSender)
            {
                DeliveryId = deliveryId;
                Message = message;
                MessageSentSender = messageSentSender;
            }

            public long DeliveryId { get; }

            public string Message { get; }
            public IActorRef MessageSentSender { get; }
        }

        public class Confirm
        {
            public Confirm(long deliveryId, IActorRef orginalSender)
            {
                DeliveryId = deliveryId;
                OrginalSender = orginalSender;
            }

            public long DeliveryId { get; }
            public IActorRef OrginalSender { get; }
        }

        public interface IEvent
        {

        }

        public class MessageSent : IEvent
        {
            public MessageSent(string message, IActorRef sender)
            {
                Message = message;
                Sender = sender;
            }

            public string Message { get; }
            public IActorRef Sender { get; }
        }

        public class MsgConfirmed : IEvent
        {
            public MsgConfirmed(long deliveryId, IActorRef orginalSender)
            {
                DeliveryId = deliveryId;
                OrginalSender = orginalSender;
            }

            public long DeliveryId { get; }
            public IActorRef OrginalSender { get; }
        }

        public class Command
        {
        }
    }
}
