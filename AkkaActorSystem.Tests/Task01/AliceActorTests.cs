using System;
using Akka.Actor;
using AkkaActorSystem.Task01;
using Xunit;

namespace AkkaActorSystem.Tests.Task01
{
    public class AliceActorTests:ActorTestBase
    {
        [Fact]
        public void WhenStartsThenSendMessage()
        {
            var props = Props.Create(() => new AliceActor(_testProbe));
            _sut = Sys.ActorOf(props);
            _testProbe.ExpectMsg<Messages.AuthRequest>();
        }


        [Fact]
        public void WhenReceivesAuthResponseThenSendNextRequest()
        {
            var props = Props.Create(() => new AliceActor(_testProbe));
            _sut = Sys.ActorOf(props);

            _sut.Tell(new Messages.AuthResponse());
            ExpectMsg<Messages.AnotherAuthRequest>();
        }


    }
}
