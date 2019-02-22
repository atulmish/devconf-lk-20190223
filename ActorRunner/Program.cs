using System;
using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.Routing;
using AkkaActorSystem.Task06;
using Serilog;

namespace ActorRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("provide option:");
            var input = Console.ReadLine();
            var doWeHaveInt = int.TryParse(input, out int value);
            if (doWeHaveInt)
            {
                Console.WriteLine(value);
                ActorSystemReference.StartSystem();
                if (value == 1)
                {
                    Console.WriteLine("starting at least one delivery example");
                    StartAtLeastOneExample();
                }

                if (value == 2)
                {
                    Console.WriteLine("starting remote deployment example");
                    StartRemoteExample();
                }

                if (value == 3)
                {
                    Console.WriteLine("starting cluster example");
                    StartClusterExample();
                }


                ActorSystemReference.ActorsSystem.WhenTerminated.Wait();
            }
        }

        private static void StartClusterExample()
        {
            var actor =
                ActorSystemReference
                    .ActorsSystem.ActorOf(
                        Props.Create(() => new ExampleAtLeastOnceDeliveryReceiveActor())
                            .WithRouter(new ClusterRouterPool(new RoundRobinPool(100),
                                new ClusterRouterPoolSettings(100, 2, false, "crawler"))),
                        "Example");

            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(i);
                    actor.Tell("Do something");
                }
            }
        }

        private static void StartRemoteExample()
        {
            Log.Information("creating actor");
            var remoteAddress = Address.Parse("akka.tcp://deployTarget@localhost:9001");
            var props = Props.Create(() => new ExampleAtLeastOnceDeliveryReceiveActor())
                .WithDeploy(Deploy.None.WithScope(new RemoteScope(remoteAddress)));

            var actor = ActorSystemReference
                .ActorsSystem
                .ActorOf(props);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i);
                actor.Tell("Do something");
            }
        }

        private static void StartAtLeastOneExample()
        {
            Log.Information("creating actor");
            var props = Props.Create(() => new ExampleAtLeastOnceDeliveryReceiveActor());
            var actor = ActorSystemReference.ActorsSystem.ActorOf(props);
            var msgCount = 0;
            for (int i = 0; i < msgCount; i++)
            {
                Console.WriteLine(i);
                actor.Tell("Do something");
            }
        }
    }
}