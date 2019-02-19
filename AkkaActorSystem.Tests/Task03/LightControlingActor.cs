using System.Collections.Generic;
using Akka.Actor;
using AkkaActorSystem.Task02;
using AkkaActorSystem.Task03;
using Xunit;

namespace AkkaActorSystem.Tests.Task03
{
    public class LightControllingActorTests:ActorTestBase
    {

        [Fact]
        public void WhenStartedAndReceivesTimerThenStateAActivated()
        {
            // arrange/given
            var props = Props.Create(() => new LightControllingActor());
            _sut = Sys.ActorOf(props);

            var actorDict = new Dictionary<string, IActorRef>
            {
                {"NS",_testProbe},
                {"EW",_testProbe},
                {"P",_testProbe},
            };

            // act/when
            _sut.Tell(new LightControlMessages.Start(actorDict));

            _testProbe.ExpectMsg<LightControlMessages.PedestriansRed>();
            _testProbe.ExpectMsg<LightControlMessages.NorthSouthRed>();
            _testProbe.ExpectMsg<LightControlMessages.EastWestRed>();

            _sut.Tell(new LightControlMessages.Timer());
            _testProbe.ExpectMsg<LightControlMessages.EastWestGreen>();
        }
    }
}
