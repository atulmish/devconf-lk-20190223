using System;
using Akka.Actor;
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
                    Console.WriteLine("starting recovery example");
                    StartRecoveryExample();

                }

                ActorSystemReference.ActorsSystem.WhenTerminated.Wait();
            }

      }

        private static void StartRecoveryExample()
        {
            Console.WriteLine("?????");
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
