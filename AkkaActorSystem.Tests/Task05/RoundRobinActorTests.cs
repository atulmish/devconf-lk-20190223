using System;
using Akka.Actor;
using Akka.Routing;
using AkkaActorSystem.Task04;
using AkkaActorSystem.Task05;
using Xunit;

namespace AkkaActorSystem.Tests.Task05
{
    public class RoundRobinActorTests : ActorTestBase
    {
        [Fact]
        public void WhenSent5MessagesResponsesShallBeIn6Seconds()
        {
            // run this test to demonstrate what is going on
            // dotnet test --filter "FullyQualifiedName=AkkaActorSystem.Tests.Task05.RoundRobinActorTests.WhenSent5MessagesResponsesShallBeIn6Seconds"

            // arrange/given
            var props = Props.Create(() => new BaseWorkerActor());
            // .WithRouter(new RoundRobinPool(5));
            _sut = Sys.ActorOf(props);
            var numberOfMessages = 5;
            // act/when

            for (int i = 0; i < numberOfMessages; i++)
            {
                _sut.Tell(new ScalingMessages.ProcessData(numberOfMessages - 1));
            }

            // assert/then
            ReceiveN(numberOfMessages, TimeSpan.FromSeconds(numberOfMessages));
        }
    }
}