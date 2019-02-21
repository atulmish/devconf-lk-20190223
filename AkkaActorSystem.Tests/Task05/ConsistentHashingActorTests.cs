using System;
using Akka.Actor;
using Akka.Routing;
using AkkaActorSystem.Task05;
using Should.Fluent;
using Xunit;

namespace AkkaActorSystem.Tests.Task05
{
    public class ConsistentHashingActorTests:ActorTestBase
    {
        [Fact]
        public void WhenSent5MessagesResponsesShallBeIn2Seconds()
        {

            // run this test to demonstrate what is going on
            // dotnet test --filter "FullyQualifiedName=AkkaActorSystem.Tests.Task05.ScatterGatherActorTests.WhenSent5MessagesResponsesShallBeIn2Seconds"

            // arrange/given
            var props = Props.Create(() => new BaseWorkerActor())
                .WithRouter(new ConsistentHashingPool(2));
            _sut = Sys.ActorOf(props);
            var numberOfMessages = 5;

            // act/when
            // assert/then

            _sut.Tell(new ScalingMessages.HashMessage("a"));

            ExpectMsg<ScalingMessages.HashRsp>(
                m => {Assert.Same("a", m.CustomerIds); }
            );


            _sut.Tell(new ScalingMessages.HashMessage("b"));

            ExpectMsg<ScalingMessages.HashRsp>(
                m => {Assert.Same("b", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("c"));

            ExpectMsg<ScalingMessages.HashRsp>(
                m => {Assert.Same("c", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("a"));

            ExpectMsg<ScalingMessages.HashRsp>(
                m => {Assert.Same("aa", m.CustomerIds); }
            );

            _sut.Tell(new ScalingMessages.HashMessage("c"));

            ExpectMsg<ScalingMessages.HashRsp>(
                m => {Assert.Same("cc", m.CustomerIds); }
            );




            ExpectNoMsg();




        }
    }
}
