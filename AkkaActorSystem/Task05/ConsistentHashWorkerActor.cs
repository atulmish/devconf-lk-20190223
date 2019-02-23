using System;
using System.Threading;
using Akka.Actor;
using Akka.Event;

namespace AkkaActorSystem.Task05
{
    public class ConsistentHashWorkerActor : ReceiveActor
    {
        private ILoggingAdapter _log = Context.GetLogger();
        private string _customerIds = string.Empty;

        public ConsistentHashWorkerActor()
        {
            Receive<ScalingMessages.HashMessage>(p =>
            {
                _log.Info($"Process start from {Sender.Path} to  {Self.Path} with customer id: {p.CustomerCode}");
                _customerIds += p.CustomerCode;

                _log.Info($"Process finished from {Sender.Path} to  {Self.Path}, customers are: {_customerIds}");
                Sender.Tell(new ScalingMessages.HashRsp(_customerIds));
            });
        }
    }
}