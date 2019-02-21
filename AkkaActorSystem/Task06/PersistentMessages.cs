namespace AkkaActorSystem.Task06
{
    public class PersistentMessages
    {


        public class WithDeliveryId
        {
            public WithDeliveryId(long deliveryId, string message)
            {
                DeliveryId = deliveryId;
                Message = message;
            }

            public long DeliveryId { get; }

            public string Message { get; }
        }

        public class Confirm
        {
            public Confirm(long deliveryId)
            {
                DeliveryId = deliveryId;
            }

            public long DeliveryId { get; }
        }

        public interface IEvent
        {

        }

        public class MessageSent : IEvent
        {
            public MessageSent(string message)
            {
                Message = message;
            }

            public string Message { get; }
        }

        public class MsgConfirmed : IEvent
        {
            public MsgConfirmed(long deliveryId)
            {
                DeliveryId = deliveryId;
            }

            public long DeliveryId { get; }
        }
    }
}
