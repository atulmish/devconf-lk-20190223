using System;
using Akka.Actor;
using Akka.Routing;
using AkkaActorSystem.Task05;
using AkkaActorSystem.Task06;
using Xunit;

namespace AkkaActorSystem.Tests.Task06
{
    public class ExampleAtLeastOnceDeliveryReceiveActorTests:ActorTestBase
    {

        [Fact]
        public void WhenWorkerDropsMessagesThenShallAllBeProcessed()
        {
            // run this test to demonstrate what is going on
            // dotnet test --filter "FullyQualifiedName=AkkaActorSystem.Tests.Task06.ExampleAtLeastOnceDeliveryReceiveActorTests.WhenWorkerDropsMessagesThenShallAllBeProcessed"

            // arrange/given
            var props = Props.Create(() => new ExampleAtLeastOnceDeliveryReceiveActor());
            _sut = Sys.ActorOf(props);

            // act/when
            var msgCount = 10;
            for (int i = 0; i < msgCount; i++)
            {
                _sut.Tell("Do something");
            }

            // assert/then
            ReceiveN(msgCount, TimeSpan.FromSeconds(50));
        }
    }
}
