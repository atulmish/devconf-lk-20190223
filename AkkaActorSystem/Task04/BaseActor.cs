using System;
using Akka.Actor;

namespace AkkaActorSystem.Task04
{
    public class BaseActor : ReceiveActor
    {
        internal int _counter;

        public BaseActor()
        {
            Receive<Messages.AddOne>(a => { _counter++; });
            Receive<Messages.GetStatus>(g => { Sender.Tell(new Messages.MyStatus(_counter)); });
            Receive<Messages.UnsafeOperation>(u =>
            {
                Console.WriteLine("blow up");
                throw new Exception("that is bad");
            });
        }
    }
}