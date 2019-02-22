using System;
using ActorRunner;

namespace ActorRemoteDeployee
{
    class Program
    {
        static void Main(string[] args)
        {
                ActorSystemReference.StartSystem();
                Console.WriteLine("Deployment Target is ready");
                ActorSystemReference.ActorsSystem.WhenTerminated.Wait();
        }
    }
}
