using Akka.Actor;
using AkkaActorSystem.Task04;
using Xunit;

namespace AkkaActorSystem.Tests.Task04
{
    public class ResumingActorTests:ActorTestBase
    {
        [Fact]
        public void WhenReceivesAddAndGetStatusThenStatusMessageIsSent()
        {
            // arrange/given
            var props = Props.Create(() => new ResumingActor());
            _sut = Sys.ActorOf(props);

            // act/when
            _sut.Tell(new Messages.AddOne());
            _sut.Tell(new Messages.GetStatus());
            ExpectMsg<Messages.MyStatus>(a =>
            {
                Assert.True(a.Value == 1);
            });

        }
    }
}
