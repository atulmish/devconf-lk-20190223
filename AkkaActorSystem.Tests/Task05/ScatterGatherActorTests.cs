using System;
using Akka.Actor;
using Akka.Routing;
using AkkaActorSystem.Task05;
using Xunit;

namespace AkkaActorSystem.Tests.Task05
{
    public class ScatterGatherActorTests:ActorTestBase
    {
        [Fact]
        public void WhenSent5MessagesResponsesShallBeIn2Seconds()
        {

            // run this test to demonstrate what is going on
            // dotnet test --filter "FullyQualifiedName=AkkaActorSystem.Tests.Task05.ScatterGatherActorTests.WhenSent5MessagesResponsesShallBeIn2Seconds"

           // arrange/given
            var props = Props.Create(() => new BaseWorkerActor())
                .WithRouter(new ScatterGatherFirstCompletedPool(5));
            _sut = Sys.ActorOf(props);
            var numberOfMessages = 5;

            // act/when


                _sut.Tell(new ScalingMessages.ProcessDataRandomSleepTime(2));


            // assert/then
            ExpectMsg<ScalingMessages.Response>(TimeSpan.FromSeconds(2));
            ExpectNoMsg();




        }
    }
}
