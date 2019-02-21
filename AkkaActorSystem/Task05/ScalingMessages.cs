using System;

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

            public ProcessData( double sleepTime)
            {
                SleepTime = sleepTime;
            }

        }


        public class ProcessDataRandomSleepTime:IProcessData
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

            public ProcessDataRandomSleepTime( double sleepBase)
            {
                _sleepBase = sleepBase;
            }

        }

        public class Response
        {
        }
    }
}
