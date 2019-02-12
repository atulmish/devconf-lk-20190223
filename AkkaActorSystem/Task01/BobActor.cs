using Akka.Actor;

namespace AkkaActorSystem.Task01
{
    public class BobActor:ReceiveActor
    {
        public BobActor()
        {
            Receive<Messages.AuthRequest>(m =>
            {
                Sender.Tell(new Messages.AuthResponse());
            });

            Receive<Messages.AnotherAuthRequest>(m =>
            {
                Sender.Tell(new Messages.AnotherAuthResponse());
            });
        }
    }
}
