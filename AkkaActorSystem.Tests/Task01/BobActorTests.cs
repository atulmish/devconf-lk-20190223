using Akka.Actor;
using AkkaActorSystem.Task01;
using Xunit;

namespace AkkaActorSystem.Tests.Task01
{
    public class BobActorTests : ActorTestBase
    {
        [Fact]
        public void WhenBobReceivesAuthenticationRequestThenResponseIsSent()
        {
            var props = Props.Create(() => new BobActor());
            _sut = Sys.ActorOf(props);

            _sut.Tell(new Messages.AuthRequest());
            ExpectMsg<Messages.AuthResponse>();
        }


        [Fact]
        public void WhenBobReceivesAnotherAuthenticationRequestThenAnotherResponseIsSent()
        {
            WhenBobReceivesAuthenticationRequestThenResponseIsSent();

            _sut.Tell(new Messages.AnotherAuthRequest());
            ExpectMsg<Messages.AnotherAuthResponse>();
        }
    }
}