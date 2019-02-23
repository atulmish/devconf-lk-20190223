using System;
using Akka.Routing;

namespace AkkaActorSystem.Task05
{
    public class ScalingMessages
    {
        public interface IProcessData
        {
            double SleepTime { get; }
        }

        public class ProcessData : IProcessData
        {
            public double SleepTime { get; }

            public ProcessData(double sleepTime)
            {
                SleepTime = sleepTime;
            }
        }


        public class ProcessDataRandomSleepTime : IProcessData
        {
            private readonly double _sleepBase;

            public double SleepTime
            {
                get
                {
                    var rnd = new Random();
                    // ReSharper disable once PossibleLossOfFraction
                    return rnd.Next((int) (_sleepBase * 0.3 * 1000), (int) (_sleepBase * 1.9 * 1000)) / 1000;
                }
            }

            public ProcessDataRandomSleepTime(double sleepBase)
            {
                _sleepBase = sleepBase;
            }
        }

        public class Response
        {
        }

        public class HashRsp
        {
            public string CustomerIds { get; }

            public HashRsp(string customerIds)
            {
                CustomerIds = customerIds;
            }
        }

        public class HashMessage : IConsistentHashable
        {
            public string CustomerCode { get; }

            public HashMessage(string customerCode)
            {
                CustomerCode = customerCode;
            }

            public object ConsistentHashKey => CustomerCode;
        }
    }
}