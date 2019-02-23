using System;
using System.Threading;
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
            Console.WriteLine("Cluster is ready");
            var actor =
                ActorSystemReference
                    .ActorsSystem.ActorOf(
                        Props.Create(() => new ExampleAtLeastOnceDeliveryReceiveActor())
                            .WithRouter(new ClusterRouterPool(new RoundRobinPool(20),
                                new ClusterRouterPoolSettings(20, 2, true, "worker"))),
                        "Example");

            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    actor.Tell("Do something");
                }

                Thread.Sleep(100);
            }

            ActorSystemReference.ActorsSystem.WhenTerminated.Wait();
        }
    }
}