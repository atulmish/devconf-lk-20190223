using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit;
using Xunit;
// ReSharper disable InconsistentNaming

namespace AkkaActorSystem.Tests
{
    public class ActorTestBase:TestKit, IDisposable
    {
#pragma warning disable 649
        internal IActorRef _sut;
#pragma warning restore 649


        protected readonly TestProbe _testProbe;

        protected ActorTestBase(): base(@"akka.loglevel = DEBUG")
        {
            _testProbe = CreateTestProbe("testProbe");
        }
        public new void Dispose()
        {
            _testProbe.Tell(PoisonPill.Instance);
            base.Dispose();
        }

    }
}
