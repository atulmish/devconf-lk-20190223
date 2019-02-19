using System;
using Akka.Actor;
using AkkaActorSystem.Task04;
using Should.Fluent;
using Xunit;

namespace AkkaActorSystem.Tests.Task04
{
    public class StoppingActorTests : ActorTestBase
    {
        [Fact]
        public void WhenChildRequestedAMessageThenActorRefIsSent()
        {
            // arrange/given
            var props = Props.Create(() => new StoppingActor());
            _sut = Sys.ActorOf(props);

            // act/when
            _sut.Tell(new Messages.CreateChild());

            // assert/then
            ExpectMsg<Messages.ChildCreated>(e => { _sut = e.Child; },TimeSpan.FromMinutes(1));
        }
        [Fact]
        public void WhenReceivesAddAndGetStatusThenStatusMessageIsSent()
        {
            // arrange/given
            WhenChildRequestedAMessageThenActorRefIsSent();

            // act/when
            _sut.Tell(new Messages.AddOne());
            _sut.Tell(new Messages.GetStatus());

            // assert/then
            ExpectMsg<Messages.MyStatus>(a => { Assert.True(a.Value == 1); });
        }


        [Fact]
        public void WhenExceptionThenCounterIsPreserved()
        {
            // arrange/when
            WhenReceivesAddAndGetStatusThenStatusMessageIsSent();

            // act/when
            Watch(_sut);
            _sut.Tell(new Messages.AddOne());
            _sut.Tell(new Messages.UnsafeOperation());
            _sut.Tell(new Messages.AddOne());

            // assert/then
            ExpectMsg<Terminated>();

        }
    }
}
