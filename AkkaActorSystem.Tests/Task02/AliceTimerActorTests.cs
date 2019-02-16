using System;
using Akka.Actor;
using AkkaActorSystem.Task01;
using AkkaActorSystem.Task02;
using Xunit;

namespace AkkaActorSystem.Tests.Task02
{

    public class AliceTimerActorTests:ActorTestBase
    {
        [Fact]
        public void WhenStartedThenSumMessageSentOnTimer()
        {
            var props = Props.Create(() => new AliceTimerActor(_testProbe));

            _sut = Sys.ActorOf(props);


            _testProbe.ExpectMsg<TimerMessages.SummarisedValue>(TimeSpan.FromSeconds(10));
        }



        [Fact]
        public void WhenReceivedAddValueThenSumMessageSentOnTimer()
        {
            // arrange/given
            var props = Props.Create(() => new AliceTimerActor(_testProbe));
            _sut = Sys.ActorOf(props);

            // act/when
            _sut.Tell(new TimerMessages.AddValue(2));
            _sut.Tell(new TimerMessages.AddValue(3));

            // assert/then
            _testProbe.ExpectMsg<TimerMessages.SummarisedValue>(a =>
            {
                Assert.Equal(5 ,a.Amount);
            },TimeSpan.FromSeconds(10));

        }

        [Fact]
        public void WhenGetPoisonPillThenShallStopMessaging()
        {
            // run this test to demonstrate that we shall cancel timers :)
            // dotnet test --filter "FullyQualifiedName=AkkaActorSystem.Tests.Task02.AliceTimerActorTests.WhenGetPoisonPillThenShallStopMessaging"

            // arrange/given
            WhenReceivedAddValueThenSumMessageSentOnTimer();

            // act/when
            _sut.Tell(PoisonPill.Instance);


            // assert/then
            _testProbe.ExpectNoMsg(TimeSpan.FromSeconds(10));

        }


    }
}
