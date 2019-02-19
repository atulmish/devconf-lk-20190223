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

        [Fact]
        public void WhenInStateAAndTimerThenStateBShallBeActivated()
        {
            // arrange/given
            WhenStartedAndReceivesTimerThenStateAActivated();

            // act/when
            _sut.Tell(new LightControlMessages.Timer());

            // assert/then
            _testProbe.ExpectMsg<LightControlMessages.EastWestRed>();
            _testProbe.ExpectMsg<LightControlMessages.NorthSouthGreen>();

        }


        [Fact]
        public void WhenInStateAAndPushButtonAndTimerThenStateBShallBeActivated()
        {
            // arrange/given
            WhenStartedAndReceivesTimerThenStateAActivated();

            // act/when
            _sut.Tell(new LightControlMessages.PedestriansPushButtonActivated());
            _sut.Tell(new LightControlMessages.Timer());

            // assert/then
            _testProbe.ExpectMsg<LightControlMessages.EastWestRed>();
            _testProbe.ExpectMsg<LightControlMessages.NorthSouthGreen>();
        }


        [Fact]
        public void WhenInStateBAndTimerNoPushButtonThenStateAShallBeActivated()
        {
            // arrange/given
            WhenInStateAAndTimerThenStateBShallBeActivated();

            // act/when
            _sut.Tell(new LightControlMessages.Timer());

            // assert/then
            _testProbe.ExpectMsg<LightControlMessages.NorthSouthRed>();
            _testProbe.ExpectMsg<LightControlMessages.EastWestGreen>();
        }


        [Fact]
        public void WhenInStateBAndTimerAndPushButtonThenStateCShallBeActivated()
        {
            // arrange/given
            WhenInStateAAndPushButtonAndTimerThenStateBShallBeActivated();

            // act/when
            _sut.Tell(new LightControlMessages.Timer());

            // assert/then
            _testProbe.ExpectMsg<LightControlMessages.NorthSouthRed>();
            _testProbe.ExpectMsg<LightControlMessages.PedestriansGreen>();
        }





        [Fact]
        public void WhenInStateCAndTimerThenStateAShallBeActivated()
        {
            // arrange/given
            WhenInStateBAndTimerAndPushButtonThenStateCShallBeActivated();

            // act/when
            _sut.Tell(new LightControlMessages.Timer());

            // assert/then
            _testProbe.ExpectMsg<LightControlMessages.PedestriansRed>();
            _testProbe.ExpectMsg<LightControlMessages.EastWestGreen>();
        }


    }
}
