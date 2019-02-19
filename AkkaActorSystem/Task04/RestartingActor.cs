using System;
using Akka.Actor;
using Akka.Routing;

namespace AkkaActorSystem.Task04
{
    public class RestartingActor:ReceiveActor
    {
        public RestartingActor()
        {
            Receive<Messages.CreateChild>(c =>
            {
                var props = Props.Create(() => new BaseActor());
                var child = Context.ActorOf(props);
                Sender.Tell(new Messages.ChildCreated(child));
            });
        }
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(r=>Directive.Restart);
        }
    }
}
