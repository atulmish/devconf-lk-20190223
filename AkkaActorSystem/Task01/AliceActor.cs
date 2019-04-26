using Akka.Actor;

namespace AkkaActorSystem.Task01
{
    public class AliceActor : ReceiveActor
    {
        private readonly IActorRef _bob;

        public AliceActor(IActorRef bob)
        {
            _bob = bob;
            Receive<Messages.AuthResponse>(m =>
            {
                Sender.Tell(new Messages.AnotherAuthRequest());
            });
        }

        protected override void PreStart()
        {
            _bob.Tell(new Messages.AuthRequest());
            base.PreStart();
        }
    }
}
