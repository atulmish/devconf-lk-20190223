using System;
using System.Diagnostics;
using System.Net;
using Akka.Actor;
using Akka.Configuration;
using Serilog;
using Serilog.Events;

namespace ActorRunner
{
    public static class ActorSystemReference
    {
        public static ActorSystem ActorsSystem;

        private static Config GetConfig()
        {
            var hostname = Dns.GetHostName();
            Console.WriteLine(hostname);
            var config = ConfigurationFactory.ParseString(
                hocon: @"
                    akka {
                        loglevel=INFO,  loggers=[""Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog""]
                        actor.provider = cluster
                    remote {
                        dot-netty.tcp {
                            port = 8081
                            hostname = localhost
                        }
                    }
                    cluster {
                        seed-nodes = [""akka.tcp://ClusterSystem@localhost:8081""]
                        roles = [master]
                        }
                    }
                    ");

            return config;
        }

        /// <summary>The main.</summary>
        /// <param name="args">The args.</param>
        public static void StartSystem()
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();


            var assembly = global::System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = fvi.FileVersion;
            Log.Information($"App version: {version}");


            var config = GetConfig();
            ActorsSystem = ActorSystem.Create("ClusterSystem", config);

        }
    }
}
