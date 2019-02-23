using System;
using ActorRunner;
using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.Routing;
using AkkaActorSystem.Task06;

namespace ActorRemoteDeployee
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystemReference.StartSystem();
            Console.WriteLine("Worker is ready");


            ActorSystemReference.ActorsSystem.WhenTerminated.Wait();
        }
    }
}