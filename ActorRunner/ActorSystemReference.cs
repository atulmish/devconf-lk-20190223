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
                    loglevel=DEBUG,  loggers=[""Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog""]
                        actor {
                            provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                        }
                        remote {
                            dot-netty.tcp {
                                port = 9000
                                hostname = ""localhost""
                                public-hostname = ""localhost""

                                  # Sets the send buffer size of the Sockets,
                                  # set to 0b for platform default
                                  send-buffer-size = 33554432b

                                  # Sets the receive buffer size of the Sockets,
                                  # set to 0b for platform default
                                  receive-buffer-size = 33554432b

                                  # Maximum message size the transport will accept, but at least
                                  # 32000 bytes.
                                  # Please note that UDP does not support arbitrary large datagrams,
                                  # so this setting has to be chosen carefully when using UDP.
                                  # Both send-buffer-size and receive-buffer-size settings has to
                                  # be adjusted to be able to buffer messages of maximum size.
                                  maximum-frame-size = 16777216b


                            }
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
            ActorsSystem = ActorSystem.Create("actorSystem", config);

        }
    }
}
