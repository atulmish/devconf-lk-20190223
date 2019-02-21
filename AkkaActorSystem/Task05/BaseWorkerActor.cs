using System;
using System.Threading;
using Akka.Actor;
using Akka.Event;

namespace AkkaActorSystem.Task05
{
    public class BaseWorkerActor:ReceiveActor
    {
        private ILoggingAdapter _log = Context.GetLogger();
        public BaseWorkerActor()
        {
            Receive<ScalingMessages.IProcessData>(p =>
            {
                _log.Info($"Process start from {Sender.Path} to  {Self.Path} wit sleep time: {p.SleepTime}");
                Thread.Sleep(TimeSpan.FromSeconds(p.SleepTime));

                _log.Info($"Process finished from {Sender.Path} to  {Self.Path}");
                Sender.Tell(new ScalingMessages.Response());
            });
        }

    }
}
