using System;
using Akka.Actor;
using AkkaActorSystem.Task04;
using Xunit;

namespace AkkaActorSystem.Tests.Task04
{
    public class RestartingActorTests : ActorTestBase
    {
        [Fact]
        public void WhenChildRequestedAMessageWithActorRefIsSent2()
        {
            // arrange/given
            var props = Props.Create(() => new RestartingActor());
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
            WhenChildRequestedAMessageWithActorRefIsSent2();

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
            _sut.Tell(new Messages.AddOne());
            _sut.Tell(new Messages.UnsafeOperation());
            _sut.Tell(new Messages.AddOne());
            _sut.Tell(new Messages.GetStatus());

            // assert/then
            ExpectMsg<Messages.MyStatus>(a => { Assert.Equal(1, a.Value); });
        }
    }
}
