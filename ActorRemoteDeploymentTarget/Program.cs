using System;
using ActorRunner;

namespace ActorRemoteDeployee
{
    class Program
    {
        static void Main(string[] args)
        {ActorSystemReference.StartSystem();
            Console.WriteLine("Hello World!");
        }
    }
}
