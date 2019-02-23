using System;
using Akka.Actor;
using Akka.Routing;
using AkkaActorSystem.Task05;
using Should.Fluent;
using Xunit;

namespace AkkaActorSystem.Tests.Task05
{
    public class ConsistentHashingActorTests : ActorTestBase
    {
        [Fact]
        public void WhenMessageSentToRouterThenShallBeProcessedOnSameWorker()
        {
            // run this test to demonstrate what is going on
            // dotnet test --filter "FullyQualifiedName=AkkaActorSystem.Tests.Task05.ConsistentHashingActorTests.WhenMessageSentToRouterThenShallBeProcessedOnSameWorker"

            // arrange/given
            var props = Props.Create(() => new ConsistentHashWorkerActor())
                .WithRouter(new ConsistentHashingPool(2));
            _sut = Sys.ActorOf(props);

            // act/when
            // assert/then
            _sut.Tell(new ScalingMessages.HashMessage("a"));
            ExpectMsg<ScalingMessages.HashRsp>(
                m => { Assert.Same("a", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("u"));
            ExpectMsg<ScalingMessages.HashRsp>(
                m => { Assert.Same("u", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("c"));
            ExpectMsg<ScalingMessages.HashRsp>(
                m => { Assert.Equal("uc", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("a"));
            ExpectMsg<ScalingMessages.HashRsp>(
                m => { Assert.Equal("aa", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("c"));
            ExpectMsg<ScalingMessages.HashRsp>(
                m => { Assert.Equal("ucc", m.CustomerIds); }
            );
        }
    }
}